using System.Linq.Dynamic.Core;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebTemplate.Application.Shared;
using WebTemplate.Shared.Extensions;
using WebTemplate.Web.Extensions;

namespace WebTemplate.Web.Web;

[GenericNameConvention]
public class GenericMvcController<TEntity, TDisplayModel, TEditModel> : Controller
  where TEntity : BaseEntity
  where TEditModel : class
{
    private readonly DbContext _dbContext;

    public GenericMvcController(DbContext dbContext)
    {
        this._dbContext = dbContext;
    }

    [HttpGet]
    public virtual async Task<IActionResult> Index([FromQuery] PaginationViewModel<TEntity> model)
    {
        try
        {
            if (ModelState.IsValid)
            {
                var query = this._dbContext.Set<TEntity>().AsNoTracking();
                if (!string.IsNullOrWhiteSpace(model.Query))
                {
                    query = query.Where(model.Query);
                }
                model.TotalCount = await query.CountAsync().ConfigureAwait(false);
                if (!string.IsNullOrWhiteSpace(model.OrderBy))
                {
                    query = query.OrderBy(model.OrderBy);
                }
                model.Items = await query.Skip(model.PageSize * (model.PageIndex - 1)).Take(model.PageSize).ToListAsync().ConfigureAwait(false);
                return this.Result(model);
            }
            return BadRequest();
        }
        catch (Exception ex)
        {
            return Problem(ex.Message);
        }
    }

    [HttpGet]
    public virtual async Task<IActionResult> Details(Guid? id)
    {
        if (id == null)
        {
            return BadRequest();
        }
        var entity = await _dbContext.Set<TEntity>().FirstOrDefaultAsync(o => o.Id == id).ConfigureAwait(false);
        if (entity == null)
        {
            return NotFound();
        }
        var model = entity?.To<TEditModel>();
        return this.Result(model);
    }

    [HttpGet]
    public virtual IActionResult Create()
    {
        var model = Activator.CreateInstance(typeof(TEditModel));
        return this.Result(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public virtual async Task<IActionResult> Create([FromBody] TEditModel model)
    {
        if (ModelState.IsValid)
        {
            var entity = model.To<TEntity>();
            this._dbContext.Set<TEntity>().Add(entity);
            await _dbContext.SaveChangesAsync().ConfigureAwait(false);
            return Ok();
        }
        return BadRequest();
    }

    [HttpGet]
    public virtual async Task<IActionResult> Edit(Guid? id)
    {
        if (id == null)
        {
            return BadRequest();
        }
        var entity = await _dbContext.Set<TEntity>().FirstOrDefaultAsync(o => o.Id == id).ConfigureAwait(false);
        if (entity == null)
        {
            return NotFound();
        }
        var model = entity?.To<TEditModel>();
        return this.Result(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public virtual async Task<IActionResult> Edit([FromBody] TEditModel model)
    {
        if (ModelState.IsValid)
        {
            var id = (Guid)(typeof(TEditModel).GetProperty("Id")?.GetValue(model)!);
            var entity = this._dbContext.Set<TEntity>().FirstOrDefault(o => o.Id == id);
            entity.From(model);
            await _dbContext.SaveChangesAsync().ConfigureAwait(false);
            return Ok();
        }
        return BadRequest();
    }

    [HttpPost]
    public virtual async Task<IActionResult> Delete(List<Guid> model)
    {
        var query = this._dbContext.Set<TEntity>();
        var entities = await query.Where(o => model.Contains(o.Id)).ToListAsync().ConfigureAwait(false);
        this._dbContext.RemoveRange(entities);
        await _dbContext.SaveChangesAsync().ConfigureAwait(false);
        return Ok();
    }
}

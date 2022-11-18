//using System.Globalization;
//using System.Linq.Dynamic.Core;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.AspNetCore.Mvc.Filters;
//using Microsoft.EntityFrameworkCore;
//using WebTemplate.Application.Shared;
//using WebTemplate.Shared.Extensions;

//namespace WebTemplate.Web.Web;

///// <summary>
///// https://github.com/dotnet/AspNetCore.Docs/blob/main/aspnetcore/mvc/advanced/app-parts/sample2
///// </summary>
///// <typeparam name="TEntity"></typeparam>
//[Route("api/[controller]")]
////[Route("api/{culture}/[controller]")]
//[ApiController]
//[GenericNameConvention]
//public class GenericApiController<TEntity, TDisplayModel, TEditModel> : Controller
//  where TEntity : BaseEntity
//  where TEditModel : class
//{
//    private readonly DbContext _dbContext;

//    public GenericApiController(DbContext dbContext)
//    {
//        this._dbContext = dbContext;
//    }

//    [HttpGet]
//    public async Task<IActionResult> Index([FromQuery] PaginationViewModel<TEntity> model)
//    {
//        try
//        {
//            if (ModelState.IsValid)
//            {
//                var query = this._dbContext.Set<TEntity>().AsNoTracking();
//                if (!string.IsNullOrWhiteSpace(model.Query))
//                {
//                    query = query.Where(model.Query);
//                }
//                model.TotalCount = await query.CountAsync();
//                if (!string.IsNullOrWhiteSpace(model.OrderBy))
//                {
//                    query = query.OrderBy(model.OrderBy);
//                }
//                model.Items = await query.Skip(model.PageSize * (model.PageIndex - 1)).Take(model.PageSize).ToListAsync();
//                return Ok(model);
//            }
//            return BadRequest();
//        }
//        catch (Exception ex)
//        {
//            return Problem(ex.Message);
//        }
//    }

//    [HttpGet("{id}")]
//    public async Task<IActionResult> Details(Guid? id)
//    {
//        var entity = await this._dbContext.Set<TEntity>().FirstOrDefaultAsync(o => o.Id == id);
//        var model = entity?.To<TEditModel>();
//        return Ok(model);
//    }

//    [HttpPost]
//    public async Task<IActionResult> Create([FromBody] TEditModel model)
//    {
//        var entity = model.To<TEntity>();
//        this._dbContext.Set<TEntity>().Add(entity);
//        await this._dbContext.SaveChangesAsync();
//        return Ok();
//    }

//    [HttpPut]
//    public async Task<IActionResult> Edit([FromBody] TEditModel model)
//    {
//        var id = (Guid)(typeof(TEditModel).GetProperty("Id")?.GetValue(model)!);
//        var entity = this._dbContext.Set<TEntity>().FirstOrDefault(o => o.Id == id);
//        entity.From(model);
//        await this._dbContext.SaveChangesAsync();
//        return Ok();
//    }

//    [HttpDelete]
//    public async Task<IActionResult> Delete(List<Guid> model)
//    {
//        var query = this._dbContext.Set<TEntity>();
//        var entities = await query.Where(o => model.Contains(o.Id)).ToListAsync();
//        this._dbContext.RemoveRange(entities);
//        await this._dbContext.SaveChangesAsync();
//        return Ok();
//    }

//    public override void OnActionExecuting(ActionExecutingContext context)
//    {
//        Response.Headers.Add("x-locale", CultureInfo.CurrentCulture.Name);
//    }
//}

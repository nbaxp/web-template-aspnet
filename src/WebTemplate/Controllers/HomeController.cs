using System.Globalization;
using System.Linq;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Localization;
using WebTemplate.Application.Email;
using WebTemplate.Application.Entities.Blog;
using WebTemplate.Application.interfaces;
using WebTemplate.Application.Interfaces;
using WebTemplate.Web.Extensions;
using WebTemplate.Web.Models;
using WebTemplate.Web.Resources;

namespace WebTemplate.Controllers;

public class HomeController : Controller
{
    private readonly IStringLocalizer<Resource> _localizer;
    private readonly IRepository<BlogPost> _blogPostRepository;
    private readonly IEmailService _emailService;

    public HomeController(IStringLocalizer<Resource> localizer, IRepository<BlogPost> blogPostRepository, IEmailService emailService)
    {
        _localizer = localizer;
        _blogPostRepository = blogPostRepository;
        _emailService = emailService;
        this._emailService.SendEmail("注册验证码", $"验证码是{DateTime.Now.Ticks}", "csurn@163.com").Wait();
    }

    public IActionResult Index(int pageIndex = 1, int pageSize = 10)
    {
        var model = _blogPostRepository.Set()
            .AsNoTracking()
            .Skip((pageIndex - 1) * pageSize)
            .Take(pageSize)
            .ToList();
        return View(model);
    }

    [HttpGet]
    public IActionResult Form()
    {
        var model = new LoginModel();
        //ModelState.Clear();
        return View(model);
    }

    [HttpPost]
    public IActionResult Form([FromForm] LoginModel model)
    {
        if (!ModelState.IsValid)
        {
            //return BadRequest(ModelState.Values);
            //return BadRequest(ModelState.ToErrors());
        }
        //var validationContext = new ValidationContext(model, Request.HttpContext.RequestServices, items: null);
        //var validationResults = new List<ValidationResult>();
        //Validator.TryValidateObject(model, validationContext, validationResults, true);

        return View(model);
    }

    [HttpGet]
    public IActionResult TestName()
    {
        return Content(Url.Action()!);
    }

    [HttpGet]
    public IActionResult Test()
    {
        ViewBag.Test = $"{CultureInfo.CurrentCulture.Name}/{CultureInfo.CurrentCulture.NativeName}:{_localizer["RequiredAttribute"]}";
        return View();
    }

    [HttpPost]
    public IActionResult Index(TestViewModel model)
    {
        return View(model);
    }

    [HttpGet]
    public IActionResult Type()
    {
        return View();
    }

    [HttpPost]
    public IActionResult Type(TypeViewModel model)
    {
        return View(model);
    }

    [HttpGet]
    public async Task<IActionResult> Valid(string userName)
    {
        await Task.Delay(1 * 1000);
        return Ok(userName == "admin");
    }

    //protected IActionResult Result<TEditModel>(object model)
    //{
    //    return Json(new
    //    {
    //        schema = this.GetJsonSchema<TEditModel>(),
    //        model,
    //        errors = ModelState.Where(o => o.Value?.ValidationState == ModelValidationState.Invalid),
    //        data = ViewData
    //    });
    //}

    private bool IsJsonRequest(ControllerBase controller)
    {
        if (controller is null)
        {
            throw new ArgumentNullException(nameof(controller));
        }

        return controller.Request.Headers["accept"].ToString().Contains("json", StringComparison.OrdinalIgnoreCase);
    }
}

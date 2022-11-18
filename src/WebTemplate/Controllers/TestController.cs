using Microsoft.AspNetCore.Mvc;
using NJsonSchema;
using NJsonSchema.Generation;
using WebTemplate.Extensions;
using WebTemplate.Models;
using WebTemplate.Web.Json;
using WebTemplate.Web.Models;

namespace WebTemplate.Controllers;

public class TestController : Controller
{
    public IActionResult Index()
    {
        var dateTimeOffsetNow = DateTimeOffset.Now;
        var model = new Class2FormModel
        {
            DateTimeOffset_Input_DateTimeLocal = dateTimeOffsetNow,
            DateTimeOffset_Date_InputDate = dateTimeOffsetNow,
            DateTimeOffset_Date_InputTime = dateTimeOffsetNow
        };
        return View(model);
    }

    [HttpPost]
    public IActionResult Index([FromBody] Class2FormModel model)
    {
        if (ModelState.IsValid)
        {
            return Ok();
        }
        return BadRequest(ModelState.ToErrors());
    }

    public IActionResult JsonSchemaNetGeneration([FromRoute] string name)
    {
        var type = AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(o => o.GetTypes())
            .Where(t => t.GetInterfaces().Any(o => o.GetType() == typeof(IViewModel)))
            .Where(t => t.Name == name)
            .FirstOrDefault();
        var schema = new { };
        return Ok(schema);
    }

    public IActionResult NJsonSchema()
    {
        var settings = new JsonSchemaGeneratorSettings
        {
            SerializerSettings = null,
            SerializerOptions = new System.Text.Json.JsonSerializerOptions
            {
                ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles,
            },
            SchemaNameGenerator = new CustomSchemaNameGenerator(),
        };
        var generator = new CustomJsonSchemaGenerator(settings);
        var schema = generator.Generate(typeof(TestViewModel));
        return Ok(schema.ToJson());
    }
}

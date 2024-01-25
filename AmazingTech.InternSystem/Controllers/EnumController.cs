using AmazingTech.InternSystem.Data.Enum;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Reflection;

namespace AmazingTech.InternSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EnumController : ControllerBase
    {
        [HttpGet("getAll")]
       
       public IActionResult GetAllEnums()
        {
            var enumTypes = typeof(Enums).GetNestedTypes(BindingFlags.Public | BindingFlags.Static)
                                       .Where(t => t.IsEnum);

            var result = new Dictionary<string, Dictionary<string, string>>();

            foreach (var enumType in enumTypes)
            {
                var enumValues = Enum.GetValues(enumType).Cast<object>()
                                     .Select((value, index) => new { Index = index + 1, Value = value })
                                     .ToDictionary(pair => pair.Index.ToString(), pair => pair.Value.ToString());

                result[enumType.Name] = enumValues;
            }

            return Ok(result);
        }
    }
}

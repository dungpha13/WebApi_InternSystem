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

            var result = new Dictionary<string, List<string>>();

            foreach (var enumType in enumTypes)
            {
                var enumValues = Enum.GetNames(enumType).ToList();
                result[enumType.Name] = enumValues;
            }

            return Ok(result);
        }
    }
}

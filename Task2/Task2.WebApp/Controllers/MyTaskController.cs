using Microsoft.AspNetCore.Mvc;

namespace Task2.WebApp.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MyTaskController : ControllerBase
    {
        [HttpPatch]
        public string GetValue([FromBody] ValueRequest valueRequest)
        {
            return string.IsNullOrEmpty(valueRequest.Value) ? "null" : valueRequest.Value;
        }
    }

    public record ValueRequest(string Name, string? Value = "none");
}

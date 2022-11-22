using System.Reflection;
using Microsoft.AspNetCore.Mvc;

namespace Autobarn.Website.Controllers.api {
    [Route("api")]
    [ApiController]
    public class DefaultController : ControllerBase {
        [HttpGet]
        public IActionResult Get() {
            var response = new {
                message = "Welcome to the Autobarn API",
                version = Assembly.GetExecutingAssembly().FullName,
                _links = new {
                    vehicles = new {
                        href = "/api/vehicles"
                    },
                    models = new {
                        href = "/api/models"
                    }
                }
            };
            return Ok(response);
        }
    }
}

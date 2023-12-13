using Microsoft.AspNetCore.Mvc;

namespace Optimal.Framework.Controller
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public abstract class BaseController : ControllerBase
    {
    }
}

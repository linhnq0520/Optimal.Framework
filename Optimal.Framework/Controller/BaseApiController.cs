using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Optimal.Framework.CQRS.Common;

namespace Optimal.Framework.Controller
{
    [ApiController]
    [Route("api/[controller]")]
    public abstract class BaseApiController : ControllerBase
    {
        private ISender _mediator;
        protected ISender Mediator =>
            _mediator ??= HttpContext.RequestServices.GetService<ISender>();

        protected async Task<ActionResult<T>> Query<T>(IQuery<T> query)
        {
            return Ok(await Mediator.Send(query));
        }

        protected async Task<ActionResult<T>> Command<T>(ICommand<T> command)
        {
            return Ok(await Mediator.Send(command));
        }

        protected async Task<ActionResult> Command(ICommand command)
        {
            await Mediator.Send(command);
            return Ok();
        }
    }
}

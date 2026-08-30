using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.Data;

namespace ms_usuario.Extensions
{
    public static class ControllerExtensions
    {
        public static async Task<ActionResult> SendAsync(this ControllerBase controller, IMediator mediator, object request)
        {
            try
            {
                return controller.Ok(await mediator.Send(request, controller.HttpContext.RequestAborted));
            }
            catch (ArgumentNullException ex)
            {
                return controller.BadRequest(ex.ParamName ?? ex.Message);
            }
            catch (SecurityTokenException ex)
            {
                return controller.StatusCode(403, ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return controller.StatusCode(403, ex.Message);
            }
            catch (DuplicateNameException ex)
            {
                return controller.Conflict(ex.Message);
            }
        }
    }
}

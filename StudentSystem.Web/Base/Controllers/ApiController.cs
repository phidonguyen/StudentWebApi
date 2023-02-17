using System.Security.Authentication;
using System.Security.Claims;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using SystemTech.Core.Exceptions;
using SystemTech.Core.Messages;

namespace StudentSystem.Web.Base.Controllers
{
    [EnableCors("APIPublicCors")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    public class ApiController : ControllerBase
    {
        protected ActionResult ResponseCollection<TResult, TField>(BaseResponse<TResult, TField> response) where TResult : class, new() where TField : class
        {
            if (response.HasErrors())
            {
                return HandleErrors(response.ErrorMessages.First());
            }

            return base.Ok(new { Data = response.Result, Total = response.Total });
        }

        protected ActionResult ResponseItem<TResult, TField>(BaseResponse<List<TResult>, TField> response) where TResult : class, new() where TField : class
        {
            if (response.HasErrors())
            {
                return HandleErrors(response.ErrorMessages.First());
            }

            return base.Ok(response.Result.First());
        }

        protected ActionResult ResponseItem<TResult, TField>(BaseResponse<TResult, TField> response) where TResult : class, new() where TField : class
        {
            if (response.HasErrors())
            {
                return HandleErrors(response.ErrorMessages.First());
            }

            return base.Ok(response.Result);
        }

        private ActionResult HandleErrors(Exception exception)
        {
            // Client
            Type type = exception.GetType();

            if (type == typeof(UnauthorizedException))
            {
                return base.Unauthorized();
            }

            if (type == typeof(FailedValidationException))
            {
                FailedValidationException failedValidationException = (FailedValidationException)exception;
                ModelState.AddModelError(failedValidationException.NameProperty, exception.Message);

                return BadRequest(ModelState);
            }
            
            if (type == typeof(RecordNotFoundException) || type == typeof(ErrorsTrackingException) 
                                                        || type == typeof(ArgumentException))
            {
                return NotFound(new { errors = exception.Message });
            }

            if (type == typeof(NothingChangesException))
            {
                return NoContent();
            }

            if (type == typeof(AuthenticationException))
            {
                return BadRequest(new { errors = exception.Message });
            }
            
            // Server
            // LogSystemRaven.WriteLogRaven(exception);
            throw exception; // throw exception to outside for catching 's global -> ServerErrorHandling
        }

        protected string GetCurrentUser()
        {
            return User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        }

        protected string GetAccessToken()
        {
            string bearerToken = HttpContext?.Request?.Headers["Authorization"] ?? "";
            if (!string.IsNullOrEmpty(bearerToken))
                return bearerToken.Replace("Bearer", "").Trim();
            return "";
        }

        protected string GetIpAddress()
        {
            return HttpContext.Connection.RemoteIpAddress?.ToString() ?? null;
        }

        protected string GetUserAgent()
        {
            return HttpContext.Request.Headers["User-Agent"].ToString() ?? null;
        }
        
    }
}
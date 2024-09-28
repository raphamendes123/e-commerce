using Core.Domain.ResponseResult;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Core.ApiConfigurations
{
    [ApiController]
    public abstract class MainControllerApi : ControllerBase
    {
        public readonly IAspNetUser _aspNetUser;

        protected ICollection<string> Errors = new List<string>();

        protected Guid usuarioId { get; set; }
        protected bool UsuarioAutenticado { get; set; }

        protected MainControllerApi(IAspNetUser aspNetUser)
        {
            _aspNetUser = aspNetUser;

            if (aspNetUser.IsAuthenticated())
            {
                usuarioId = aspNetUser.GetUserId();
                UsuarioAutenticado = true;
            }
        }
        protected ActionResult CustomResponse(object result = null)
        {
            if (OperacaoInvalida())
            {
                return Ok(result);
            }
            else
            {
                return BadRequest(error:
                    new ValidationProblemDetails(
                        errors: new Dictionary<string, string[]>() {
                        { "messages", Errors.ToArray()}
                    }));
            }
        }

        protected ActionResult CustomResponse(ICollection<string> errors)
        {
            if (!errors.Any())
            {
                return Ok(errors);
            }
            else
            {
                return BadRequest(error:
                    new ValidationProblemDetails(
                        errors: new Dictionary<string, string[]>() {
                        { "messages", errors.ToArray()}
                    }));
            }
        }
        protected bool OperacaoInvalida()
        {
            return !Errors.Any();
        }
        protected ActionResult CustomResponse(ValidationResult result)
        {
            if (!result.IsValid)
            {
                foreach (ValidationFailure error in result.Errors)
                {
                    AddError(error.ErrorMessage);
                }
            }
            return CustomResponse();
        }

        protected ActionResult CustomResponse(ModelStateDictionary modelState)
        {
            if (!modelState.IsValid)
            {
                var erros = modelState.Values.SelectMany(e => e.Errors);

                foreach (var erro in erros)
                {
                    var errorMessage = erro.Exception == null ? erro.ErrorMessage : erro.Exception.Message;
                    AddError(errorMessage);
                }
            }
            return CustomResponse();
        }

        protected void AddError(string errorMessage)
        {
            Errors.Add(errorMessage);
        }

        protected void ClearErrors(string errorMessage)
        {
            Errors.Clear();
        } 

        protected ActionResult CustomResponse(ResponseResult response)
        {
            if (response != null && response.Errors.Messages.Any())
            {
                foreach (string error in response.Errors.Messages)
                {
                    AddError(error); 
                } 
            }

            return CustomResponse();
        }

        protected bool ContainsErrors(ResponseResult response)
        {
            return response?.Errors?.Messages?.Count > 0 ? true : false;
        }

        protected void CleanErrors()
        {
            Errors.Clear();
        }
    }
}

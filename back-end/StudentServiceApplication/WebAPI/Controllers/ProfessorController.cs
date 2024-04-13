using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.ServiceFabric.Services.Remoting.Client;
using Communication;
using Models.DTO;
using Models.Common;

namespace WebAPI.Controllers
{
    [Route("api/professor")]
    [ApiController]
    public class ProfessorController : ControllerBase
    {
        [HttpPost]
        [Route("signin")]
        public async Task<ActionResult> SignIn([FromBody] ProfessorSignInDTO professorSignInDTO)
        {
            try
            {
                var statelessValidationServiceProxy = ServiceProxy.Create<IValidation>(
                    new Uri("fabric:/StudentServiceApplication/ValidationService"));
                var res1 = await statelessValidationServiceProxy.ValidateProfessorSignIn(professorSignInDTO);
                if (res1 == false)
                    return StatusCode(400, new { Error = "Sign in failed!" });
                var statelessAuthenticationServiceProxy = ServiceProxy.Create<IAuthentication>(
                    new Uri("fabric:/StudentServiceApplication/AuthenticationService"));
                var res2 = await statelessAuthenticationServiceProxy.AuthenticateProfessor(professorSignInDTO);
                if (res2 == false)
                    return StatusCode(400, new { Error = "Sign in failed!" });
                var token = await statelessAuthenticationServiceProxy.IssueTokenForProfessor();

                return Ok(token);
            }
            catch (Exception e)
            {
                return StatusCode(500, new { Error = "Internal Server Error: " + e.Message });
            }
        }
    }
}

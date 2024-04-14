using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.ServiceFabric.Services.Remoting.Client;
using Communication;
using Models.DTO;
using Models.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.ServiceFabric.Services.Client;
using System.Data;
using System.Fabric;

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
                var token = await statelessAuthenticationServiceProxy.IssueTokenForProfessor(professorSignInDTO.Email);

                return Ok(token);
            }
            catch (Exception e)
            {
                return StatusCode(500, new { Error = "Internal Server Error: " + e.Message });
            }
        }

        [HttpGet]
        [Route("by-email/{email}")]
        [Authorize(Roles = "professor")]
        public async Task<ActionResult> GetProfessor(string email)
        {
            try
            {
                var statefulServiceUri = new Uri("fabric:/StudentServiceApplication/ProfessorService");
                FabricClient client = new FabricClient();
                var statefulServicePartitionKeyList = await client.QueryManager.GetPartitionListAsync(statefulServiceUri);
                var partitionKey = new ServicePartitionKey((statefulServicePartitionKeyList[0].PartitionInformation as Int64RangePartitionInformation).LowKey);
                var statefullProxy = ServiceProxy.Create<IProfessor>(statefulServiceUri, partitionKey);
                var professor = await statefullProxy.GetProfesorByEmail(email);

                return Ok(professor);
            }
            catch (Exception e)
            {
                return StatusCode(500, new { Error = "Internal Server Error: " + e.Message });
            }
        }

        [HttpPut]
        [Route("update")]
        [Authorize(Roles = "professor")]
        public async Task<ActionResult> Update(ProfessorUpdateDTO professorUpdateDTO)
        {
            try
            {
                //prepare
                var statelessServiceProxy = ServiceProxy.Create<ITransactionCoordinator>(
                    new Uri("fabric:/StudentServiceApplication/TransactionCoordinatorService"));
                var professor = await statelessServiceProxy.PrepareUpdateProfessor(professorUpdateDTO);

                //commit
                await statelessServiceProxy.CommitProfessor();

                return Ok(professor);
            }
            catch (Exception e)
            {    
                var statelessServiceProxy = ServiceProxy.Create<ITransactionCoordinator>(
                    new Uri("fabric:/StudentServiceApplication/TransactionCoordinatorService"));
                //rollback
                await statelessServiceProxy.RollbackProfessor();

                return StatusCode(500, new { Error = "Internal Server Error: " + e.Message });
            }
        }
    }
}

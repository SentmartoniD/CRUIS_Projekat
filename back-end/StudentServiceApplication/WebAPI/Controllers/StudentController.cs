using Communication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.ServiceFabric.Services.Client;
using Microsoft.ServiceFabric.Services.Remoting.Client;
using Models.DTO;
using System.Fabric;

namespace WebAPI.Controllers
{
    [Route("api/student")]
    [ApiController]
    public class StudentController : ControllerBase
    {
        [HttpPost]
        [Route("signin")]
        public async Task<ActionResult> SignIn([FromBody] StudentSignInDTO studentSignInDTO)
        {
            try
            {
                var statelessValidationServiceProxy = ServiceProxy.Create<IValidation>(
                    new Uri("fabric:/StudentServiceApplication/ValidationService"));
                var res1 = await statelessValidationServiceProxy.ValidateStudentSignIn(studentSignInDTO);
                if (res1 == false)
                    return StatusCode(400, new { Error = "Sign in failed!" });
                var statelessAuthenticationServiceProxy = ServiceProxy.Create<IAuthentication>(
                    new Uri("fabric:/StudentServiceApplication/AuthenticationService"));
                var res2 = await statelessAuthenticationServiceProxy.AuthenticateStudent(studentSignInDTO);
                if (res2 == false)
                    return StatusCode(400, new { Error = "Sign in failed!" });
                var token = await statelessAuthenticationServiceProxy.IssueTokenForStudent(studentSignInDTO.IndexNumber);

                return Ok(token);
            }
            catch (Exception e)
            {
                return StatusCode(500, new { Error = "Internal Server Error: " + e.Message });
            }
        }


        [HttpPost]
        [Route("signup")]
        public async Task<ActionResult> SignUp([FromBody] StudentSignUpDTO studentSignUpDTO)
        {
            try
            {
                var statelessValidationServiceProxy = ServiceProxy.Create<IValidation>(
                    new Uri("fabric:/StudentServiceApplication/ValidationService"));
                var res = await statelessValidationServiceProxy.ValidateStudentSignUp(studentSignUpDTO);
                if (res == false)
                    return StatusCode(400, new { Error = "Sign up failed!" });
                var statefulServiceUri = new Uri("fabric:/StudentServiceApplication/StudentService");
                FabricClient client = new FabricClient();
                var statefulServicePartitionKeyList = await client.QueryManager.GetPartitionListAsync(statefulServiceUri);
                var partitionKey = new ServicePartitionKey((statefulServicePartitionKeyList[0].PartitionInformation as Int64RangePartitionInformation).LowKey);
                var statefullProxy = ServiceProxy.Create<IStudent>(statefulServiceUri, partitionKey);
                var newStudent = await statefullProxy.AddStudent(studentSignUpDTO);

                return Ok(new { Data = newStudent });
            }
            catch (Exception e)
            {
                return StatusCode(500, new { Error = "Internal Server Error: " + e.Message });
            }
        }
    }
}

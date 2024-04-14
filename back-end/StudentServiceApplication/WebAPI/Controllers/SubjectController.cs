using Communication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.ServiceFabric.Services.Client;
using Microsoft.ServiceFabric.Services.Remoting.Client;
using Models.DTO;
using System.Data;
using System.Fabric;

namespace WebAPI.Controllers
{
    [Route("api/subject")]
    [ApiController]
    public class SubjectController : ControllerBase
    {
        [HttpGet]
        [Route("athended")]
        [Authorize(Roles = "student")]
        public async Task<ActionResult> GetAthended(string indexNUmber)
        {
            try
            {
                var statefulServiceUri = new Uri("fabric:/StudentServiceApplication/StudentService");
                FabricClient client = new FabricClient();
                var statefulServicePartitionKeyList = await client.QueryManager.GetPartitionListAsync(statefulServiceUri);
                var partitionKey = new ServicePartitionKey((statefulServicePartitionKeyList[0].PartitionInformation as Int64RangePartitionInformation).LowKey);
                var statefullProxy = ServiceProxy.Create<IStudent>(statefulServiceUri, partitionKey);
                var student = await statefullProxy.GetStudent(indexNUmber);

                statefulServiceUri = new Uri("fabric:/StudentServiceApplication/SubjectService");
                client = new FabricClient();
                statefulServicePartitionKeyList = await client.QueryManager.GetPartitionListAsync(statefulServiceUri);
                partitionKey = new ServicePartitionKey((statefulServicePartitionKeyList[0].PartitionInformation as Int64RangePartitionInformation).LowKey);
                var statefullProxySubject = ServiceProxy.Create<ISubject>(statefulServiceUri, partitionKey);
                var subjects = await statefullProxySubject.GetAthendedSubjectsForStudent(student.Id);

                return Ok(subjects);
            }
            catch (Exception e)
            {
                return StatusCode(500, new { Error = "Internal Server Error: " + e.Message });
            }
        }
    }
}

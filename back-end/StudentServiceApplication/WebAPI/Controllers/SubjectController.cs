using Communication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.ServiceFabric.Services.Client;
using Microsoft.ServiceFabric.Services.Remoting.Client;
using Models.Common;
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
        [Route("athended/{indexNumber}")]
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

        [HttpGet]
        [Route("athended-remove/{subjectId}/{indexNumber}")]
        [Authorize(Roles = "student")]
        public async Task<ActionResult> RemoveAthendedSubject(int subjectId, string indexNUmber)
        {
            try
            {
                var statefulServiceUri = new Uri("fabric:/StudentServiceApplication/StudentService");
                FabricClient client = new FabricClient();
                var statefulServicePartitionKeyList = await client.QueryManager.GetPartitionListAsync(statefulServiceUri);
                var partitionKey = new ServicePartitionKey((statefulServicePartitionKeyList[0].PartitionInformation as Int64RangePartitionInformation).LowKey);
                var statefullProxy = ServiceProxy.Create<IStudent>(statefulServiceUri, partitionKey);
                var student = await statefullProxy.GetStudent(indexNUmber);

                //prepare
                var statelessServiceProxy = ServiceProxy.Create<ITransactionCoordinator>(
                    new Uri("fabric:/StudentServiceApplication/TransactionCoordinatorService"));
                await statelessServiceProxy.PrepareDeleteStudentFromSubject(subjectId, student.Id);

                //commit
                await statelessServiceProxy.CommitSubject();

                return Ok();
            }
            catch (Exception e)
            {
                var statelessServiceProxy = ServiceProxy.Create<ITransactionCoordinator>(
                    new Uri("fabric:/StudentServiceApplication/TransactionCoordinatorService"));
                //rollback
                await statelessServiceProxy.RollbackSubject();

                return StatusCode(500, new { Error = "Internal Server Error: " + e.Message });
            }
        }

        [HttpGet]
        [Route("unathended/{indexNumber}")]
        [Authorize(Roles = "student")]
        public async Task<ActionResult> GetUnAthended(string indexNUmber)
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
                var subjects = await statefullProxySubject.GetUnathendedSubjectsForStudent(student.Id);

                return Ok(subjects);
            }
            catch (Exception e)
            {
                return StatusCode(500, new { Error = "Internal Server Error: " + e.Message });
            }
        }

        [HttpGet]
        [Route("unathended-add/{subjectId}/{indexNumber}")]
        [Authorize(Roles = "student")]
        public async Task<ActionResult> AddUnAthendedSubject(int subjectId, string indexNUmber)
        {
            try
            {
                var statefulServiceUri = new Uri("fabric:/StudentServiceApplication/StudentService");
                FabricClient client = new FabricClient();
                var statefulServicePartitionKeyList = await client.QueryManager.GetPartitionListAsync(statefulServiceUri);
                var partitionKey = new ServicePartitionKey((statefulServicePartitionKeyList[0].PartitionInformation as Int64RangePartitionInformation).LowKey);
                var statefullProxy = ServiceProxy.Create<IStudent>(statefulServiceUri, partitionKey);
                var student = await statefullProxy.GetStudent(indexNUmber);

                //prepare
                var statelessServiceProxy = ServiceProxy.Create<ITransactionCoordinator>(
                    new Uri("fabric:/StudentServiceApplication/TransactionCoordinatorService"));
                await statelessServiceProxy.PrepareAddStudentToSubject(subjectId, student.Id);

                //commit
                await statelessServiceProxy.CommitSubject();

                return Ok();
            }
            catch (Exception e)
            {
                var statelessServiceProxy = ServiceProxy.Create<ITransactionCoordinator>(
                        new Uri("fabric:/StudentServiceApplication/TransactionCoordinatorService"));
                //rollback
                await statelessServiceProxy.RollbackSubject();

                return StatusCode(500, new { Error = "Internal Server Error: " + e.Message });
            }
        }

        [HttpGet]
        [Route("professor-all/{email}")]
        [Authorize(Roles = "professor")]
        public async Task<ActionResult> GetSubjectsForProfessor(string email)
        {
            try
            {
                var statefulServiceUri = new Uri("fabric:/StudentServiceApplication/ProfessorService");
                FabricClient client = new FabricClient();
                var statefulServicePartitionKeyList = await client.QueryManager.GetPartitionListAsync(statefulServiceUri);
                var partitionKey = new ServicePartitionKey((statefulServicePartitionKeyList[0].PartitionInformation as Int64RangePartitionInformation).LowKey);
                var statefullProxy = ServiceProxy.Create<IProfessor>(statefulServiceUri, partitionKey);
                var professor = await statefullProxy.GetProfessor(email);

                statefulServiceUri = new Uri("fabric:/StudentServiceApplication/SubjectService");
                client = new FabricClient();
                statefulServicePartitionKeyList = await client.QueryManager.GetPartitionListAsync(statefulServiceUri);
                partitionKey = new ServicePartitionKey((statefulServicePartitionKeyList[0].PartitionInformation as Int64RangePartitionInformation).LowKey);
                var statefullProxySubject = ServiceProxy.Create<ISubject>(statefulServiceUri, partitionKey);
                var subjects = await statefullProxySubject.GetSubjectsForProfessor(professor.Id);

                return Ok(subjects);
            }
            catch (Exception e)
            {
                return StatusCode(500, new { Error = "Internal Server Error: " + e.Message });
            }
        }

        [HttpGet]
        [Route("change-grade/{subjectId}/{studentId}/{grade}")]
        [Authorize(Roles = "professor")]
        public async Task<ActionResult> ChangeGrade(int subjectId, int studentId, int grade )
        {
            try
            {
                //prepare
                var statelessServiceProxy = ServiceProxy.Create<ITransactionCoordinator>(
                    new Uri("fabric:/StudentServiceApplication/TransactionCoordinatorService"));
                await statelessServiceProxy.PrepareChangeGrade(subjectId, studentId, grade);

                //commit
                await statelessServiceProxy.CommitSubject();

                return Ok();
            }
            catch (Exception e)
            {
                var statelessServiceProxy = ServiceProxy.Create<ITransactionCoordinator>(
                        new Uri("fabric:/StudentServiceApplication/TransactionCoordinatorService"));
                //rollback
                await statelessServiceProxy.RollbackSubject();

                return StatusCode(500, new { Error = "Internal Server Error: " + e.Message });
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Fabric;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Communication;
using Microsoft.ServiceFabric.Services.Client;
using Microsoft.ServiceFabric.Services.Communication.Runtime;
using Microsoft.ServiceFabric.Services.Remoting.Client;
using Microsoft.ServiceFabric.Services.Runtime;
using Models.Common;
using Models.DTO;

namespace TransactionCoordinatorService
{
    /// <summary>
    /// An instance of this class is created for each service instance by the Service Fabric runtime.
    /// </summary>
    internal sealed class TransactionCoordinatorService : StatelessService, ITransactionCoordinator
    {
        public TransactionCoordinatorService(StatelessServiceContext context)
            : base(context)
        { }

        public async Task CommitProfessor()
        {
            var statefulServiceUri = new Uri("fabric:/StudentServiceApplication/ProfessorService");
            FabricClient client = new FabricClient();
            var statefulServicePartitionKeyList = await client.QueryManager.GetPartitionListAsync(statefulServiceUri);
            var partitionKey = new ServicePartitionKey((statefulServicePartitionKeyList[0].PartitionInformation as Int64RangePartitionInformation).LowKey);
            var statefullProxy = ServiceProxy.Create<IProfessor>(statefulServiceUri, partitionKey);
            await statefullProxy.UpdateState();
        }

        public async Task CommitStudent()
        {
            var statefulServiceUri = new Uri("fabric:/StudentServiceApplication/StudentService");
            FabricClient client = new FabricClient();
            var statefulServicePartitionKeyList = await client.QueryManager.GetPartitionListAsync(statefulServiceUri);
            var partitionKey = new ServicePartitionKey((statefulServicePartitionKeyList[0].PartitionInformation as Int64RangePartitionInformation).LowKey);
            var statefullProxy = ServiceProxy.Create<IStudent>(statefulServiceUri, partitionKey);
            await statefullProxy.UpdateState();
        }

        public async Task CommitSubject()
        {
            var statefulServiceUri = new Uri("fabric:/StudentServiceApplication/SubjectService");
            FabricClient client = new FabricClient();
            var statefulServicePartitionKeyList = await client.QueryManager.GetPartitionListAsync(statefulServiceUri);
            var partitionKey = new ServicePartitionKey((statefulServicePartitionKeyList[0].PartitionInformation as Int64RangePartitionInformation).LowKey);
            var statefullProxy = ServiceProxy.Create<ISubject>(statefulServiceUri, partitionKey);
            await statefullProxy.UpdateState();
        }

        public async Task<Student> PrepareAddStudent(StudentSignUpDTO studentSignUpDTO)
        {
            var statefulServiceUri = new Uri("fabric:/StudentServiceApplication/StudentService");
            FabricClient client = new FabricClient();
            var statefulServicePartitionKeyList = await client.QueryManager.GetPartitionListAsync(statefulServiceUri);
            var partitionKey = new ServicePartitionKey((statefulServicePartitionKeyList[0].PartitionInformation as Int64RangePartitionInformation).LowKey);
            var statefullProxy = ServiceProxy.Create<IStudent>(statefulServiceUri, partitionKey);
            var student = await statefullProxy.AddStudent(studentSignUpDTO);
            return student;
        }

        public async Task PrepareAddStudentToSubject(int subjectId, int studentId)
        {
            var statefulServiceUri = new Uri("fabric:/StudentServiceApplication/SubjectService");
            FabricClient client = new FabricClient();
            var statefulServicePartitionKeyList = await client.QueryManager.GetPartitionListAsync(statefulServiceUri);
            var partitionKey = new ServicePartitionKey((statefulServicePartitionKeyList[0].PartitionInformation as Int64RangePartitionInformation).LowKey);
            var statefullProxy = ServiceProxy.Create<ISubject>(statefulServiceUri, partitionKey);
            await statefullProxy.AddStudentToSubject(subjectId, studentId);
            
        }

        public async Task PrepareChangeGrade(int subjectId, int studentId, int grade)
        {
            var statefulServiceUri = new Uri("fabric:/StudentServiceApplication/SubjectService");
            FabricClient client = new FabricClient();
            var statefulServicePartitionKeyList = await client.QueryManager.GetPartitionListAsync(statefulServiceUri);
            var partitionKey = new ServicePartitionKey((statefulServicePartitionKeyList[0].PartitionInformation as Int64RangePartitionInformation).LowKey);
            var statefullProxy = ServiceProxy.Create<ISubject>(statefulServiceUri, partitionKey);
            await statefullProxy.ChangeGrade(subjectId, studentId, grade);
        }

        public async Task PrepareDeleteStudentFromSubject(int subjectId, int studentId)
        {
            var statefulServiceUri = new Uri("fabric:/StudentServiceApplication/SubjectService");
            FabricClient client = new FabricClient();
            var statefulServicePartitionKeyList = await client.QueryManager.GetPartitionListAsync(statefulServiceUri);
            var partitionKey = new ServicePartitionKey((statefulServicePartitionKeyList[0].PartitionInformation as Int64RangePartitionInformation).LowKey);
            var statefullProxy = ServiceProxy.Create<ISubject>(statefulServiceUri, partitionKey);
            await statefullProxy.DeleteStudentFromSubject(subjectId, studentId);
        }

        public async Task<Professor> PrepareUpdateProfessor(ProfessorUpdateDTO professorUpdateDTO)
        {
            var statefulServiceUri = new Uri("fabric:/StudentServiceApplication/ProfessorService");
            FabricClient client = new FabricClient();
            var statefulServicePartitionKeyList = await client.QueryManager.GetPartitionListAsync(statefulServiceUri);
            var partitionKey = new ServicePartitionKey((statefulServicePartitionKeyList[0].PartitionInformation as Int64RangePartitionInformation).LowKey);
            var statefullProxy = ServiceProxy.Create<IProfessor>(statefulServiceUri, partitionKey);
            var professor = await statefullProxy.UpdateProfessor(professorUpdateDTO);
            return professor;
        }

        public async Task<Student> PrepareUpdateStudent(StudentUpdateDTO studentUpdateDTO)
        {
            var statefulServiceUri = new Uri("fabric:/StudentServiceApplication/StudentService");
            FabricClient client = new FabricClient();
            var statefulServicePartitionKeyList = await client.QueryManager.GetPartitionListAsync(statefulServiceUri);
            var partitionKey = new ServicePartitionKey((statefulServicePartitionKeyList[0].PartitionInformation as Int64RangePartitionInformation).LowKey);
            var statefullProxy = ServiceProxy.Create<IStudent>(statefulServiceUri, partitionKey);
            var student = await statefullProxy.UpdateStudent(studentUpdateDTO);
            return student;
        }

        public async Task RollbackProfessor()
        {
            var statefulServiceUri = new Uri("fabric:/StudentServiceApplication/ProfessorService");
            FabricClient client = new FabricClient();
            var statefulServicePartitionKeyList = await client.QueryManager.GetPartitionListAsync(statefulServiceUri);
            var partitionKey = new ServicePartitionKey((statefulServicePartitionKeyList[0].PartitionInformation as Int64RangePartitionInformation).LowKey);
            var statefullProxy = ServiceProxy.Create<IProfessor>(statefulServiceUri, partitionKey);
            await statefullProxy.ReverseState();
        }

        public async Task RollbackStudent()
        {
            var statefulServiceUri = new Uri("fabric:/StudentServiceApplication/StudentService");
            FabricClient client = new FabricClient();
            var statefulServicePartitionKeyList = await client.QueryManager.GetPartitionListAsync(statefulServiceUri);
            var partitionKey = new ServicePartitionKey((statefulServicePartitionKeyList[0].PartitionInformation as Int64RangePartitionInformation).LowKey);
            var statefullProxy = ServiceProxy.Create<IStudent>(statefulServiceUri, partitionKey);
            await statefullProxy.ReverseState();
        }

        public async Task RollbackSubject()
        {
            var statefulServiceUri = new Uri("fabric:/StudentServiceApplication/SubjectService");
            FabricClient client = new FabricClient();
            var statefulServicePartitionKeyList = await client.QueryManager.GetPartitionListAsync(statefulServiceUri);
            var partitionKey = new ServicePartitionKey((statefulServicePartitionKeyList[0].PartitionInformation as Int64RangePartitionInformation).LowKey);
            var statefullProxy = ServiceProxy.Create<ISubject>(statefulServiceUri, partitionKey);
            await statefullProxy.ReverseState();
        }

        /// <summary>
        /// Optional override to create listeners (e.g., TCP, HTTP) for this service replica to handle client or user requests.
        /// </summary>
        /// <returns>A collection of listeners.</returns>
        protected override IEnumerable<ServiceInstanceListener> CreateServiceInstanceListeners()
        {
            return new ServiceInstanceListener[0];
        }

        /// <summary>
        /// This is the main entry point for your service instance.
        /// </summary>
        /// <param name="cancellationToken">Canceled when Service Fabric needs to shut down this service instance.</param>
        protected override async Task RunAsync(CancellationToken cancellationToken)
        {
            // TODO: Replace the following sample code with your own logic 
            //       or remove this RunAsync override if it's not needed in your service.

            long iterations = 0;

            while (true)
            {
                cancellationToken.ThrowIfCancellationRequested();

                ServiceEventSource.Current.ServiceMessage(this.Context, "Working-{0}", ++iterations);

                await Task.Delay(TimeSpan.FromSeconds(1), cancellationToken);
            }
        }
    }
}

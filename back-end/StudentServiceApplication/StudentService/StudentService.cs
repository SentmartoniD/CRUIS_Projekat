using System;
using System.Collections.Generic;
using System.Fabric;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.ServiceFabric.Data.Collections;
using Microsoft.ServiceFabric.Services.Communication.Runtime;
using Microsoft.ServiceFabric.Services.Runtime;
using Models.Common;
using Communication;
using Models.DTO;
using Microsoft.ServiceFabric.Services.Remoting.Runtime;

namespace StudentService
{
    /// <summary>
    /// An instance of this class is created for each service replica by the Service Fabric runtime.
    /// </summary>
    internal sealed class StudentService : StatefulService, IStudent
    {
        public StudentService(StatefulServiceContext context)
            : base(context)
        { }

        private async Task Initialize()
        {
            var students = new List<Student>
            {
                new Student
                {
                    Id = 1,
                    FirstName = "Ana",
                    LastName = "Ivanovic",
                    IndexNumber = "PR44/2019",
                    Email = "ivanovic@uns.ac.rs",
                    Password = "ivanovic123",
                    SubjectIds = new List<int>{ 1, 2, 3, 4},
                },
                new Student
                {
                    Id = 2,
                    FirstName = "Novak",
                    LastName = "Djokovic",
                    IndexNumber = "PR46/2019",
                    Email = "djokovic@uns.ac.rs",
                    Password = "djokovic123",
                    SubjectIds = new List<int>{ 1, 2, 3, 4},
                },
                new Student
                {
                    Id = 3,
                    FirstName = "Donald",
                    LastName = "Trump",
                    IndexNumber = "PR74/2019",
                    Email = "trump@uns.ac.rs",
                    Password = "trump123",
                    SubjectIds = new List<int>{ 1, 2, 3, 4},
                },
                new Student
                {
                    Id = 4,
                    FirstName = "Lionel",
                    LastName = "Messi",
                    IndexNumber = "PR101/2019",
                    Email = "messi@uns.ac.rs",
                    Password = "messi123",
                    SubjectIds = new List<int>{ 1, 2, 3, 4},
                },
                new Student
                {
                    Id = 5,
                    FirstName = "Cristiano",
                    LastName = "Ronaldo",
                    IndexNumber = "PR11/2019",
                    Email = "ronaldo@uns.ac.rs",
                    Password = "ronaldo123",
                    SubjectIds = new List<int>{ 1, 2, 3, 4},
                },
            };

            var prevStudentDictionary = await this.StateManager.GetOrAddAsync<IReliableDictionary<int, Student>>("prevStudentDictionary");

            using (var tx = this.StateManager.CreateTransaction())
            {
                foreach (var student in students)
                {
                    await prevStudentDictionary.AddAsync(tx, student.Id, student);
                }
                await tx.CommitAsync();
            }

            var currentStudentDictionary = await this.StateManager.GetOrAddAsync<IReliableDictionary<int, Student>>("currentStudentDictionary");

            using (var tx = this.StateManager.CreateTransaction())
            {
                foreach (var student in students)
                {
                    await currentStudentDictionary.AddAsync(tx, student.Id, student);
                }
                await tx.CommitAsync();
            }
        }

        /// <summary>
        /// Optional override to create listeners (e.g., HTTP, Service Remoting, WCF, etc.) for this service replica to handle client or user requests.
        /// </summary>
        /// <remarks>
        /// For more information on service communication, see https://aka.ms/servicefabricservicecommunication
        /// </remarks>
        /// <returns>A collection of listeners.</returns>
        protected override IEnumerable<ServiceReplicaListener> CreateServiceReplicaListeners()
        {
            return this.CreateServiceRemotingReplicaListeners();
        }

        /// <summary>
        /// This is the main entry point for your service replica.
        /// This method executes when this replica of your service becomes primary and has write status.
        /// </summary>
        /// <param name="cancellationToken">Canceled when Service Fabric needs to shut down this service replica.</param>
        protected override async Task RunAsync(CancellationToken cancellationToken)
        {
            // TODO: Replace the following sample code with your own logic 
            //       or remove this RunAsync override if it's not needed in your service.

            var myDictionary = await this.StateManager.GetOrAddAsync<IReliableDictionary<string, long>>("myDictionary");

            await Initialize();

            while (true)
            {
                cancellationToken.ThrowIfCancellationRequested();

                using (var tx = this.StateManager.CreateTransaction())
                {
                    var result = await myDictionary.TryGetValueAsync(tx, "Counter");

                    ServiceEventSource.Current.ServiceMessage(this.Context, "Current Counter Value: {0}",
                        result.HasValue ? result.Value.ToString() : "Value does not exist.");

                    await myDictionary.AddOrUpdateAsync(tx, "Counter", 0, (key, value) => ++value);

                    // If an exception is thrown before calling CommitAsync, the transaction aborts, all changes are 
                    // discarded, and nothing is saved to the secondary replicas.
                    await tx.CommitAsync();
                }

                await Task.Delay(TimeSpan.FromSeconds(1), cancellationToken);
            }
        }

        public async Task<bool> CheckStudent(StudentSignInDTO studentSignInDTO)
        {
            var currentStudentDictionary = await this.StateManager.GetOrAddAsync<IReliableDictionary<int, Student>>("currentStudentDictionary");

            using var transaction = this.StateManager.CreateTransaction();
            var studentEnumerator = (await currentStudentDictionary.CreateEnumerableAsync(transaction)).GetAsyncEnumerator();

            while (await studentEnumerator.MoveNextAsync(CancellationToken.None))
            {
                var student = studentEnumerator.Current;
                if (student.Value.IndexNumber == studentSignInDTO.IndexNumber && student.Value.Password == studentSignInDTO.Password)
                    return true;
            }

            return false;
        }
    }
}

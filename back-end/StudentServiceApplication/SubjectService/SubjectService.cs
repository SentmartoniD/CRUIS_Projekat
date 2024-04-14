using System;
using System.Collections.Generic;
using System.Fabric;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Communication;
using Microsoft.ServiceFabric.Data.Collections;
using Microsoft.ServiceFabric.Services.Client;
using Microsoft.ServiceFabric.Services.Communication.Runtime;
using Microsoft.ServiceFabric.Services.Remoting.Client;
using Microsoft.ServiceFabric.Services.Remoting.Runtime;
using Microsoft.ServiceFabric.Services.Runtime;
using Models.Common;
using Models.DTO;

namespace SubjectService
{
    /// <summary>
    /// An instance of this class is created for each service replica by the Service Fabric runtime.
    /// </summary>
    internal sealed class SubjectService : StatefulService, ISubject
    {
        public SubjectService(StatefulServiceContext context)
            : base(context)
        { }

        private async Task Initialize()
        {
            var subjects = new List<Subject>
            {
                new Subject
                {
                    Id = 1,
                    Name = "Cloud Racunarstvo u Infrastrukturnim Sistemima",
                    Year = 5,
                    ProfessorId = 2,
                    StudentIds = new List<int>{ 1},
                    StudentGrades = new List<int>{ 8},
                },
                new Subject
                {
                    Id = 2,
                    Name = "Inteligentni Softveri u Infrastrukturnim Sistemima",
                    Year = 5,
                    ProfessorId = 1,
                    StudentIds = new List<int>{ 1, 2, 3, 4},
                    StudentGrades = new List<int>{ 5, 5, 9, 9},
                },
                new Subject
                {
                    Id = 3,
                    Name = "Matematicka Analiza",
                    Year = 1,
                    ProfessorId = 3,
                    StudentIds = new List<int>(),
                    StudentGrades = new List<int>(),
                },
                new Subject
                {
                    Id = 4,
                    Name = "Namenski Racunarski Sistemi",
                    Year = 3,
                    ProfessorId = 1,
                    StudentIds = new List<int>{2,3,4,5,1},
                    StudentGrades = new List<int>{9,10,7,10,5},
                }
            };

            var prevSubjectDictionary = await this.StateManager.GetOrAddAsync<IReliableDictionary<int, Subject>>("prevSubjectDictionary");

            using (var tx = this.StateManager.CreateTransaction())
            {
                foreach (var subject in subjects)
                {
                    await prevSubjectDictionary.AddAsync(tx, subject.Id, subject);
                }
                await tx.CommitAsync();
            }

            var currentSubjectDictionary = await this.StateManager.GetOrAddAsync<IReliableDictionary<int, Subject>>("currentSubjectDictionary");

            using (var tx = this.StateManager.CreateTransaction())
            {
                foreach (var subject in subjects)
                {
                    await currentSubjectDictionary.AddAsync(tx, subject.Id, subject);
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

            Initialize();

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

        public async Task<List<SubjectFullDTO>> GetSubjectsForProfessor(int professorId)
        {
            var statefulServiceUri = new Uri("fabric:/StudentServiceApplication/StudentService");
            FabricClient client = new FabricClient();
            var statefulServicePartitionKeyList = await client.QueryManager.GetPartitionListAsync(statefulServiceUri);
            var partitionKey = new ServicePartitionKey((statefulServicePartitionKeyList[0].PartitionInformation as Int64RangePartitionInformation).LowKey);
            var statefullProxy = ServiceProxy.Create<IStudent>(statefulServiceUri, partitionKey);
            var students = await statefullProxy.GetStudents();

            List<SubjectFullDTO> subjectList = new List<SubjectFullDTO>();

            var currentSubjectDictionary = await this.StateManager.GetOrAddAsync<IReliableDictionary<int, Subject>>("currentSubjectDictionary");

            using var transaction = this.StateManager.CreateTransaction();
            var subjectEnumerator = (await currentSubjectDictionary.CreateEnumerableAsync(transaction)).GetAsyncEnumerator();

            List<int> grades = null;
            Student tempStudent = null;

            while (await subjectEnumerator.MoveNextAsync(CancellationToken.None))
            {
                var subject = subjectEnumerator.Current;
                List<Student> studentList = new List<Student>();

                if (subject.Value.ProfessorId == professorId)
                {
                    for (int j = 0; j < subject.Value.StudentIds.Count; j++) {
                        var stud = students.Where(s => s.Id == subject.Value.StudentIds[j]).ToList();
                        studentList.Add(new Student { Id = stud[0].Id,  FirstName = stud[0].FirstName, LastName = stud[0].LastName,
                        IndexNumber = stud[0].IndexNumber, Email = stud[0].Email
                        });
                    }
                    subjectList.Add(new SubjectFullDTO
                    {
                        Id = subject.Key,
                        Name = subject.Value.Name,
                        Year = subject.Value.Year,
                        ProfessorId = subject.Value.ProfessorId,
                        Students = studentList,
                        StudentGrades = subject.Value.StudentGrades,
                    });
                }
            }

            return subjectList;
        }

        public async Task<List<SubjectAthendedDTO>> GetUnathendedSubjectsForStudent(int studentId)
        {
            var statefulServiceUri = new Uri("fabric:/StudentServiceApplication/ProfessorService");
            FabricClient client = new FabricClient();
            var statefulServicePartitionKeyList = await client.QueryManager.GetPartitionListAsync(statefulServiceUri);
            var partitionKey = new ServicePartitionKey((statefulServicePartitionKeyList[0].PartitionInformation as Int64RangePartitionInformation).LowKey);
            var statefullProxy = ServiceProxy.Create<IProfessor>(statefulServiceUri, partitionKey);
            var professors = await statefullProxy.GetProfessors();

            List<SubjectAthendedDTO> subjectList = new List<SubjectAthendedDTO>();

            var currentSubjectDictionary = await this.StateManager.GetOrAddAsync<IReliableDictionary<int, Subject>>("currentSubjectDictionary");

            using var transaction = this.StateManager.CreateTransaction();
            var subjectEnumerator = (await currentSubjectDictionary.CreateEnumerableAsync(transaction)).GetAsyncEnumerator();

            while (await subjectEnumerator.MoveNextAsync(CancellationToken.None))
            {
                var subject = subjectEnumerator.Current;
                if (!subject.Value.StudentIds.Contains(studentId)) 
                {
                    var professor = professors.Where(p => p.Id == subject.Value.ProfessorId).ToList();
                    subjectList.Add(new SubjectAthendedDTO
                    {
                        Id = subject.Key,
                        Name = subject.Value.Name,
                        Year = subject.Value.Year,
                        Professor = new Professor
                        {
                            Id = professor[0].Id,
                            Email = professor[0].Email,
                            FirstName = professor[0].FirstName,
                            LastName = professor[0].LastName,
                            Password = professor[0].Password,
                            SubjectIds = professor[0].SubjectIds
                        },
                        Grade = 5,
                    });
                }
            }

            return subjectList;
        }

        public async Task<List<SubjectAthendedDTO>> GetAthendedSubjectsForStudent(int studentId)
        {
            var statefulServiceUri = new Uri("fabric:/StudentServiceApplication/ProfessorService");
            FabricClient client = new FabricClient();
            var statefulServicePartitionKeyList = await client.QueryManager.GetPartitionListAsync(statefulServiceUri);
            var partitionKey = new ServicePartitionKey((statefulServicePartitionKeyList[0].PartitionInformation as Int64RangePartitionInformation).LowKey);
            var statefullProxy = ServiceProxy.Create<IProfessor>(statefulServiceUri, partitionKey);
            var professors = await statefullProxy.GetProfessors();

            List<SubjectAthendedDTO> subjectList = new List<SubjectAthendedDTO>();

            var currentSubjectDictionary = await this.StateManager.GetOrAddAsync<IReliableDictionary<int, Subject>>("currentSubjectDictionary");

            using var transaction = this.StateManager.CreateTransaction();
            var subjectEnumerator = (await currentSubjectDictionary.CreateEnumerableAsync(transaction)).GetAsyncEnumerator();

            while (await subjectEnumerator.MoveNextAsync(CancellationToken.None))
            {
                var subject = subjectEnumerator.Current;
                for (int i = 0; i < subject.Value.StudentIds.Count; i++) {
                    if (subject.Value.StudentIds[i] == studentId)
                    {
                        var professor = professors.Where(p => p.Id == subject.Value.ProfessorId).ToList();
                        subjectList.Add(new SubjectAthendedDTO
                        {
                            Id = subject.Key,
                            Name = subject.Value.Name,
                            Year = subject.Value.Year,
                            Professor = new Professor { 
                                Id = professor[0].Id, Email = professor[0].Email,
                                FirstName = professor[0].FirstName, LastName = professor[0].LastName,
                                Password = professor[0].Password, SubjectIds = professor[0].SubjectIds},
                            Grade = subject.Value.StudentGrades[i],
                        });
                    }
                }
            }

            return subjectList;
        }

        public async Task AddStudentToSubject(int subjectId, int studentId)
        {
            var currentSubjectDictionary = await this.StateManager.GetOrAddAsync<IReliableDictionary<int, Subject>>("currentSubjectDictionary");

            Subject newSubject = null;
            int maxId = 0;

            using var transaction = this.StateManager.CreateTransaction();
            var subjectEnumerator = (await currentSubjectDictionary.CreateEnumerableAsync(transaction)).GetAsyncEnumerator();

            while (await subjectEnumerator.MoveNextAsync(CancellationToken.None))
            {
                var subject = subjectEnumerator.Current;
                if (subject.Key == subjectId)
                {
                    newSubject = subject.Value;
                    break;
                }
            }
            
            newSubject.StudentIds.Add(studentId);
            newSubject.StudentGrades.Add(5);

            using (var tx = this.StateManager.CreateTransaction())
            {
                await currentSubjectDictionary.TryRemoveAsync(tx, newSubject.Id);

                await currentSubjectDictionary.AddAsync(tx, newSubject.Id, newSubject);

                await tx.CommitAsync();
            }

            return;
        }

        public async Task DeleteStudentFromSubject(int subjectId, int studentId)
        {
            var currentSubjectDictionary = await this.StateManager.GetOrAddAsync<IReliableDictionary<int, Subject>>("currentSubjectDictionary");

            Subject newSubject = null;

            using var transaction = this.StateManager.CreateTransaction();
            var subjectEnumerator = (await currentSubjectDictionary.CreateEnumerableAsync(transaction)).GetAsyncEnumerator();

            while (await subjectEnumerator.MoveNextAsync(CancellationToken.None))
            {
                var subject = subjectEnumerator.Current;
                if (subject.Key == subjectId)
                {
                    newSubject = subject.Value;
                    break;
                }
            }
            for (int i = 0; i < newSubject.StudentIds.Count; i++) {
                if (newSubject.StudentIds[i] == studentId) 
                {
                    newSubject.StudentIds.RemoveAt(i);
                    newSubject.StudentGrades.RemoveAt(i);
                }
            }

            using (var tx = this.StateManager.CreateTransaction())
            {
                await currentSubjectDictionary.TryRemoveAsync(tx, newSubject.Id);

                await currentSubjectDictionary.AddAsync(tx, newSubject.Id, newSubject);

                await tx.CommitAsync();
            }

            return ;
        }

        public async Task ChangeGrade(int subjectId, int studentId, int grade)
        {
            var currentSubjectDictionary = await this.StateManager.GetOrAddAsync<IReliableDictionary<int, Subject>>("currentSubjectDictionary");

            Subject newSubject = null;

            using var transaction = this.StateManager.CreateTransaction();
            var subjectEnumerator = (await currentSubjectDictionary.CreateEnumerableAsync(transaction)).GetAsyncEnumerator();

            while (await subjectEnumerator.MoveNextAsync(CancellationToken.None))
            {
                var subject = subjectEnumerator.Current;
                if (subject.Key == subjectId)
                {
                    newSubject = subject.Value;
                    break;
                }
            }
            for (int i = 0; i < newSubject.StudentIds.Count; i++)
            {
                if (newSubject.StudentIds[i] == studentId)
                {
                    newSubject.StudentGrades[i]=grade;
                }
            }

            using (var tx = this.StateManager.CreateTransaction())
            {
                await currentSubjectDictionary.TryRemoveAsync(tx, newSubject.Id);

                await currentSubjectDictionary.AddAsync(tx, newSubject.Id, newSubject);

                await tx.CommitAsync();
            }
        }

        public async Task UpdateState()
        {
            var currentSubjectDictionary = await this.StateManager.GetOrAddAsync<IReliableDictionary<int, Subject>>("currentSubjectDictionary");

            var prevSubjectDictionary = await this.StateManager.GetOrAddAsync<IReliableDictionary<int, Subject>>("prevSubjectDictionary");


            using (var tx = this.StateManager.CreateTransaction())
            {
                await prevSubjectDictionary.ClearAsync();
                await tx.CommitAsync();
            }

            using var transaction = this.StateManager.CreateTransaction();
            var subjectEnumerator = (await currentSubjectDictionary.CreateEnumerableAsync(transaction)).GetAsyncEnumerator();

            while (await subjectEnumerator.MoveNextAsync(CancellationToken.None))
            {
                var subject = subjectEnumerator.Current;
                await prevSubjectDictionary.AddAsync(transaction, subject.Key, subject.Value);
                await transaction.CommitAsync();
            }
        }

        public async Task ReverseState()
        {
            var currentSubjectDictionary = await this.StateManager.GetOrAddAsync<IReliableDictionary<int, Subject>>("currentSubjectDictionary");

            var prevSubjectDictionary = await this.StateManager.GetOrAddAsync<IReliableDictionary<int, Subject>>("prevSubjectDictionary");


            using (var tx = this.StateManager.CreateTransaction())
            {
                await currentSubjectDictionary.ClearAsync();
                await tx.CommitAsync();
            }

            using var transaction = this.StateManager.CreateTransaction();
            var subjectEnumerator = (await prevSubjectDictionary.CreateEnumerableAsync(transaction)).GetAsyncEnumerator();

            while (await subjectEnumerator.MoveNextAsync(CancellationToken.None))
            {
                var subject = subjectEnumerator.Current;
                await currentSubjectDictionary.AddAsync(transaction, subject.Key, subject.Value);
                await transaction.CommitAsync();
            }
        }
    }
}

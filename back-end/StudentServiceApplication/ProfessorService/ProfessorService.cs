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

namespace ProfessorService
{
    /// <summary>
    /// An instance of this class is created for each service replica by the Service Fabric runtime.
    /// </summary>
    internal sealed class ProfessorService : StatefulService
    {
        public ProfessorService(StatefulServiceContext context)
            : base(context)
        { }

        private async Task Initialize()
        {
            var professors = new List<Professor>
            {
                new Professor
                {
                    Id = 1,
                    FirstName = "Aleksandar",
                    LastName = "Selakov",
                    Email = "selakov@uns.ac.rs",
                    Password = "selakov123",
                    SubjectIds = new List<int>{ 2, 4},
                },
                new Professor
                {
                    Id = 2,
                    FirstName = "Srdjan",
                    LastName = "Vukmirovic",
                    Email = "vukmirovic@uns.ac.rs",
                    Password = "vukmirovic123",
                    SubjectIds = new List<int>{ 1},
                },
                new Professor
                {
                    Id = 3,
                    FirstName = "Lidija",
                    LastName = "Comic",
                    Email = "comic@uns.ac.rs",
                    Password = "comic123",
                    SubjectIds = new List<int>{ 3},
                }
            };

            var prevProfessorDictionary = await this.StateManager.GetOrAddAsync<IReliableDictionary<int, Professor>>("prevProfessorDictionary");

            using (var tx = this.StateManager.CreateTransaction())
            {
                foreach (var professor in professors)
                {
                    await prevProfessorDictionary.AddAsync(tx, professor.Id, professor);
                }
                await tx.CommitAsync();
            }

            var currentProfessorDictionary = await this.StateManager.GetOrAddAsync<IReliableDictionary<int, Professor>>("currentProfessorDictionary");

            using (var tx = this.StateManager.CreateTransaction())
            {
                foreach (var professor in professors)
                {
                    await currentProfessorDictionary.AddAsync(tx, professor.Id, professor);
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
            return new ServiceReplicaListener[0];
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
    }
}
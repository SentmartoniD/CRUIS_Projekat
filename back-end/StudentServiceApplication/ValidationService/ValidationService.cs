using System;
using System.Collections.Generic;
using System.Fabric;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.ServiceFabric.Services.Communication.Runtime;
using Microsoft.ServiceFabric.Services.Runtime;
using Models;
using Communication;
using Models.DTO;
using Microsoft.ServiceFabric.Services.Remoting.Runtime;

namespace ValidationService
{
    /// <summary>
    /// An instance of this class is created for each service instance by the Service Fabric runtime.
    /// </summary>
    internal sealed class ValidationService : StatelessService, IValidation
    {
        public ValidationService(StatelessServiceContext context)
            : base(context)
        { }

        public async Task<bool> ValidateStudentSignIn(StudentSignInDTO studentSignInDTO)
        {
            if (string.IsNullOrEmpty(studentSignInDTO.IndexNumber))
                return false;
            else if (string.IsNullOrEmpty(studentSignInDTO.Password))
                return false;
            else
                return true;
        }

        public async Task<bool> ValidateStudentSignUp(StudentSignUpDTO studentSignUpDTO)
        {
            if (string.IsNullOrEmpty(studentSignUpDTO.FirstName))
                return false;
            else if (string.IsNullOrEmpty(studentSignUpDTO.LastName))
                return false;
            else if (string.IsNullOrEmpty(studentSignUpDTO.IndexNumber))
                return false;
            else if (string.IsNullOrEmpty(studentSignUpDTO.Email))
                return false;
            else if (string.IsNullOrEmpty(studentSignUpDTO.Password))
                return false;
            else
                return true;
        }

        public async Task<bool> ValidateProfessorSignIn(ProfessorSignInDTO professorSignInDTO)
        {
            if (string.IsNullOrEmpty(professorSignInDTO.Email))
                return false;
            else if (string.IsNullOrEmpty(professorSignInDTO.Password))
                return false;
            else
                return true;
        }

        /// <summary>
        /// Optional override to create listeners (e.g., TCP, HTTP) for this service replica to handle client or user requests.
        /// </summary>
        /// <returns>A collection of listeners.</returns>
        protected override IEnumerable<ServiceInstanceListener> CreateServiceInstanceListeners()
        {
            return this.CreateServiceRemotingInstanceListeners();
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

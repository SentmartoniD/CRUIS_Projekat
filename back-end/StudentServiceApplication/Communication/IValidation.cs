using Microsoft.ServiceFabric.Services.Remoting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using Models.DTO;

namespace Communication
{
    [ServiceContract]
    public interface IValidation : IService
    {
        [OperationContract]
        Task<bool> ValidateStudentSignIn(StudentSignInDTO studentSignInDTO);

        [OperationContract]
        Task<bool> ValidateStudentSignUp(StudentSignUpDTO studentSignUpDTO);

        [OperationContract]
        Task<bool> ValidateProfessorSignIn(ProfessorSignInDTO professorSignInDTO);
    }
}

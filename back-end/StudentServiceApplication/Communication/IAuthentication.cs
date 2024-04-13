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
    public interface IAuthentication : IService
    {
        //student
        [OperationContract]
        Task<bool> AuthenticateStudent(StudentSignInDTO studentSignInDTO);

        [OperationContract]
        Task<string> IssueTokenForStudent(string indexNUmber);

        //professor
        [OperationContract]
        Task<bool> AuthenticateProfessor(ProfessorSignInDTO professorSignInDTO);

        [OperationContract]
        Task<string> IssueTokenForProfessor(string email);
    }
}

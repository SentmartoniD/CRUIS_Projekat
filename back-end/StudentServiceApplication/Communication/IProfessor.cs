using Microsoft.ServiceFabric.Services.Remoting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using Models.DTO;
using Models.Common;

namespace Communication
{
    [ServiceContract]
    public interface IProfessor : IService
    {
        [OperationContract]
        Task<bool> CheckProfessor(ProfessorSignInDTO professorSignInDTO);

        [OperationContract]
        Task<Professor> GetProfesor(string email);

        [OperationContract]
        Task<Professor> UpdateProfessor(ProfessorUpdateDTO professorUpdateDTO);
    }
}

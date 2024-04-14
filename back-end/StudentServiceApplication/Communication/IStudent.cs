using Microsoft.ServiceFabric.Services.Remoting;
using Models.DTO;
using Models.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Communication
{
    [ServiceContract]
    public interface IStudent : IService
    {
        [OperationContract]
        Task<bool> CheckStudent(StudentSignInDTO studentSignInDTO);

        [OperationContract]
        Task<Student> AddStudent(StudentSignUpDTO studentSignUpDTO);

        [OperationContract]
        Task<Student> GetStudent(string indexNumber);

        [OperationContract]
        Task<Student> UpdateStudent(StudentUpdateDTO studentUpdateDTO);

        [OperationContract]
        Task<List<Student>> GetStudents();

        [OperationContract]
        Task UpdateState();

        [OperationContract]
        Task ReverseState();
    }
}

using Microsoft.ServiceFabric.Services.Remoting;
using Models.Common;
using Models.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Communication
{
    [ServiceContract]
    public interface ITransactionCoordinator : IService
    {
        //student

        [OperationContract]
        Task<Student> AddStudent(StudentSignUpDTO studentSignUpDTO);

        [OperationContract]
        Task<Student> UpdateStudent(StudentUpdateDTO studentUpdateDTO);

        [OperationContract]
        Task CommitStudent();

        [OperationContract]
        Task RollbackStudent();

        //professor

        [OperationContract]
        Task<Professor> UpdateProfessor(ProfessorUpdateDTO professorUpdateDTO);

        [OperationContract]
        Task CommitProfessor();

        [OperationContract]
        Task RollbackProfessor();

        //subject

        [OperationContract]
        Task AddStudentToSubject(int subjectId, int studentId);

        [OperationContract]
        Task DeleteStudentFromSubject(int subjectId, int studentId);

        [OperationContract]
        Task ChangeGrade(int subjectId, int studentId, int grade);

        [OperationContract]
        Task CommitSubject();

        [OperationContract]
        Task RollbackSubject();
    }
}

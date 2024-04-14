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
        Task<Student> PrepareAddStudent(StudentSignUpDTO studentSignUpDTO);

        [OperationContract]
        Task<Student> PrepareUpdateStudent(StudentUpdateDTO studentUpdateDTO);

        [OperationContract]
        Task CommitStudent();

        [OperationContract]
        Task RollbackStudent();

        //professor

        [OperationContract]
        Task<Professor> PrepareUpdateProfessor(ProfessorUpdateDTO professorUpdateDTO);

        [OperationContract]
        Task CommitProfessor();

        [OperationContract]
        Task RollbackProfessor();

        //subject

        [OperationContract]
        Task PrepareAddStudentToSubject(int subjectId, int studentId);

        [OperationContract]
        Task PrepareDeleteStudentFromSubject(int subjectId, int studentId);

        [OperationContract]
        Task PrepareChangeGrade(int subjectId, int studentId, int grade);

        [OperationContract]
        Task CommitSubject();

        [OperationContract]
        Task RollbackSubject();
    }
}

using Microsoft.ServiceFabric.Services.Remoting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Models.Common;
using Models.DTO;

namespace Communication
{
    [ServiceContract]
    public interface ISubject : IService
    {
        [OperationContract]
        Task<List<Subject>> GetSubjectsForProfessor(int professorId);

        [OperationContract]
        Task<List<Subject>> GetUnathendedSubjectsForStudent(int studentId);

        [OperationContract]
        Task<List<SubjectAthendedDTO>> GetAthendedSubjectsForStudent(int studentId);


        [OperationContract]
        Task<Subject> UpdateGradesForSubject(int subjectId, List<int> grades);


        [OperationContract]
        Task AddStudentToSubject(int subjectId, int studentId);

        [OperationContract]
        Task DeleteStudentFromSubject(int subjectId, int studentId);

    }
}

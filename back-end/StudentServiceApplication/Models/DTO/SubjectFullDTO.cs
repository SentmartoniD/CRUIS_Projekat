using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Models.Common;

namespace Models.DTO
{
    [DataContract]
    public class SubjectFullDTO
    {
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public int Year { get; set; }
        [DataMember]
        public int ProfessorId { get; set; }
        [DataMember]
        public List<Student> Students { get; set; }
        [DataMember]
        public List<int> StudentGrades { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Models.Common
{
    [DataContract]
    public class Subject
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
        public List<int> StudentIds { get; set; }
        [DataMember]
        public List<int> StudentGrades { get; set; }

    }
}

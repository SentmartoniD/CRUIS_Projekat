using Models.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Models.DTO
{
    [DataContract]
    public class SubjectAthendedDTO
    {
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public int Year { get; set; }
        [DataMember]
        public Professor Professor { get; set; }
        [DataMember]
        public int Grade { get; set; }
    }
}

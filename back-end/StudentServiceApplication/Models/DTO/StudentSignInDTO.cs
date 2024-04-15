using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Models.DTO
{
    [DataContract]
    public class StudentSignInDTO
    {
        [DataMember]
        public string IndexNumber { get; set; }

        [DataMember]
        public string Password { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Models.Common;

namespace Models.DTO
{
    public class SubjectFullDTO
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int Year { get; set; }

        public Professor Professor { get; set; }

        public List<Student> Students { get; set; }

        public List<int> StudentGrades { get; set; }
    }
}

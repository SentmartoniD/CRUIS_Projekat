using Models.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.DTO
{
    public class SubjectDTO
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int Year { get; set; }

        public int ProfessorId { get; set; }

        public List<Student> Students { get; set; }

        public List<int> StudentGrades { get; set; }
    }
}

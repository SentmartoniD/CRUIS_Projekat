﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Common
{
    public class Subject
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int Year { get; set; }

        public int ProfessorId { get; set; }

        public List<int> StudentIds { get; set; }

        public List<int> StudentGrades { get; set; }

    }
}

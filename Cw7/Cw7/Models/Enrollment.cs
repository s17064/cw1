﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cw7.Models
{
    public class Enrollment
    {
        public int IdEnrollment { get; set; }
        public int Semeter { get; set; }
        public int IdStudy { get; set; }
        public DateTime StartDate { get; set; }
        public string Study { get; set; }
    }
}

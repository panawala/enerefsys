﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EnerefsysDAL.Model
{
    public class FitResult
    {
        public int FitResultID { get; set; }
        public float B1 { get; set; }
        public float B2 { get; set; }
        public float B3 { get; set; }
        public float B4 { get; set; }
        public float B5 { get; set; }
        public float B6 { get; set; }
        public double Temperature { get; set; }
    }
}

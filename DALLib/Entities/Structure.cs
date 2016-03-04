﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DALLib.Entities
{
    public class Structure
    {
        public int Id { get; set; }
        public string Path { get; set; }
        public string Name { get; set; }
        public Project Project { get; set; }
        //Add InspectionDrawing link
    }
}

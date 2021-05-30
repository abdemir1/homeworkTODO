﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace todoHW.Models
{
    public class Category
    {
        public int ID { get; set; }
        
        [MaxLength(50)]
        public string Name { get; set; }

        public virtual List<ToDo> ToDos { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace todoHW.Models
{
    public class ToDo
    {
        public int ID { get; set; }

        public string Title { get; set; }

        public bool isCompleted { get; set; }

        public DateTime DueDate { get; set; }

        public DateTime CompletedDate { get; set; }

        public DateTime CreatedDate { get; set; }

        public int CategoryID { get; set; }

        public Category Category { get; set; }

    }
}

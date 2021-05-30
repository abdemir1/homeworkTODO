using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace todoHW.Models
{
    public class ToDo
    {
        public ToDo()
        {
            CreatedDate = DateTime.Now;
        }
        public int ID { get; set; }
        [Required(ErrorMessage = "Please enter a title")]
        [MaxLength(200)]

        public string Title { get; set; }
        [MaxLength(1500)]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }

        [Display(Name = "Is Completed?")]
        public bool isCompleted { get; set; }

        public DateTime DueDate { get; set; }

        [ScaffoldColumn(false)]

        public DateTime CompletedDate { get; set; }

        public DateTime CreatedDate { get; set; }

        public int RemainingHour
        {
            get
            {
                var remainingtime = (DateTime.Now - DueDate);
                    return (int)remainingtime.TotalHours;
            }
        }

        public int CategoryID { get; set; }

        public virtual Category Category { get; set; }

    }
}

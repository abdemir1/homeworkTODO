using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace todoHW.Models
{
    

        public class SearchViewModel
        {
            public string SearchText { get; set; }
            public bool ShowAll { get; set; }

            public List<ToDo> Result { get; set; }

        }


    
}

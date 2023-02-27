using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseInterop.Models
{
    public class UserTaskMap
    {
        public int UserTaskMapID { get; set; }
        public int UserID { get; set; }
        public int TaskID { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseInterop.Models
{
    public class ApiAuthCodeTableModel
    {
        public int ApiAuthCodeTableID { get; set; }
        public int UserID { get; set; }
        public DateTime CreationDate { get; set; } = DateTime.Now;
        public string Code { get; set; } = string.Empty;
    }
}

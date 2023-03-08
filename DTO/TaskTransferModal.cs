using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTO
{
    public class TaskTransferModal
    {
        public int TaskID { get; set; }
		[Required]
		[StringLength(50, ErrorMessage = "Title must be less than 50 characters.")]

		public string Title { get; set; } = "";
		[Required]
		[StringLength(500, ErrorMessage = "Description must be less than 500 characters")]
		public string Description { get; set; } = "";
        public TaskRecurrenceTransferModal? recurrenceOptions { get; set; }
    }
}


using Sabio.Models.Domain.Users;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Sabio.Models.Domain.FAQs
{
    public class FaqModel
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public string Question { get; set; }
        [Required]
        public string Answer { get; set; }
        [Required]
        public int SortOrder { get; set; }
        [Required]
        public DateTime DateCreated { get; set; }
        [Required]
        public DateTime DateModified { get; set; }
        [Required]
        public BasicUser CreatedBy { get; set; }
        [Required]
        public BasicUser ModifiedBy { get; set; }
        [Required]
        public LookUp Category { get; set; }

    }
}





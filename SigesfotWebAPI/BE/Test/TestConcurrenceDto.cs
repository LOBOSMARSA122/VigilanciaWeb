using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BE.Test
{
    [Table("testcorrunce")]
    public class TestConcurrenceDto
    {
        [Key]
        public string TestConcurrenceId { get; set; }
        public Guid Value { get; set; }
    }
}

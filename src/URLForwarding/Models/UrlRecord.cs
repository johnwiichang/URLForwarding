using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace URLForwarding.Models
{
    public class UrlRecord
    {
        [Key]
        public String Short { get; set; }

        [Required]
        [DataType(DataType.Url)]
        public string UrlData { get; set; } = "http://";

        [Required]
        public int Counter { get; set; } = 0;

        [Required]
        [DataType(DataType.DateTime)]
        public DateTime CreateTime { get; set; } = DateTime.Now;

        [Required]
        public bool IsEnable { get; set; } = true;
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Dating_Api.DTOs
{
    public class PhotoFromReturnDto
    {
        public int Id { get; set; }
        public string Url { get; set; }
        public string Description { get; set; }
        public DateTime CreationDate { get; set; }
        public bool IsMain { get; set; }
    }
}

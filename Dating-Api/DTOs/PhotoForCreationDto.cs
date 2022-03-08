using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Dating_Api.DTOs
{
    public class PhotoForCreationDto
    {
        public PhotoForCreationDto()
        {
            CreationDate = DateTime.Now;
        }

        public string Url { get; set; }
        public IFormFile File { get; set; }
        public string  Description { get; set; }
        public DateTime  CreationDate { get; set; }
        public string  PublicId { get; set; }
    }
}

using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace escafandra.services.Application.DTOs
{
    public class PdfUploadDto
    {
        public IFormFile? File { get; set; }
    }
}

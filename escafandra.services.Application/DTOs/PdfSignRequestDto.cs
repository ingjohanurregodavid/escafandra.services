using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace escafandra.services.Application.DTOs
{
    public class PdfSignRequestDto
    {
        public int? DocumentId { get; set; }  
        public string? SignedBy { get; set; }
    }
}

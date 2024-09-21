using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace escafandra.services.Application.DTOs
{
    public class PdfResponseDto
    {
        public int Id { get; set; }            
        public string? FileName { get; set; }   
        public byte[]? FileData { get; set; }
        public bool IsSigned { get; set; }

        public DateTime UploadDate { get; set; }
    }
}

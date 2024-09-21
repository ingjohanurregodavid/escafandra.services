using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace escafandra.services.Domain.Entities
{
    public class PDFDocument
    {
        [Key] // Indica que este campo es la clave primaria
        public int Id { get; set; }

        [Required] 
        [MaxLength(255)] 
        public string? FileName { get; set; }

        [Required] 
        public byte[] FileData { get; set; }

        public DateTime UploadDate { get; set; } 

        public bool IsSigned { get; set; } 

        [MaxLength(100)] 
        public string? SignedBy { get; set; } 

        public DateTime? SignedDate { get; set; }


        public string? Signature { get; set; } 
    }
}

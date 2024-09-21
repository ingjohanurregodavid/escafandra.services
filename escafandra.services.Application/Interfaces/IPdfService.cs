using escafandra.services.Application.DTOs;
using escafandra.services.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace escafandra.services.Application.Interfaces
{
    public interface IPdfService
    {
        Task<PdfResponseDto> UploadPdfAsync(PdfUploadDto uploadDto);
        Task<PdfResponseDto> DownloadPdfAsync(int id);
        Task<PdfResponseDto> SignPdfAsync(int id);
        Task<IEnumerable<PDFDocument>> GetAll();
        Task<PdfResponseDto> GetPdfByIdAsync(int id);
    }
}

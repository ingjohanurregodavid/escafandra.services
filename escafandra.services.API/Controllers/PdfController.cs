using escafandra.services.Application.DTOs;
using escafandra.services.Application.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace escafandra.services.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PdfController : ControllerBase
    {
        private readonly IPdfService _pdfService;

        public PdfController(IPdfService pdfService)
        {
            _pdfService = pdfService;
        }

        [HttpPost("upload")]
        public async Task<IActionResult> Upload([FromForm] PdfUploadDto file)
        {
            var result = await _pdfService.UploadPdfAsync(file);
            return Ok(result); 
        }

        [HttpGet("download/{id}")]  
        public async Task<IActionResult> Download(int id)
        {
            var pdf = await _pdfService.GetPdfByIdAsync(id);
            return File(pdf.FileData, "application/pdf", pdf.FileName); 
        }

        [HttpGet("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            var result = await _pdfService.GetAll();
            return Ok(result);
        }

        [HttpPost("sign/{id}")]
        public async Task<IActionResult> Sign(int id)
        {
            var signedPdf = await _pdfService.SignPdfAsync(id);
            return Ok(signedPdf); 
        }
    }
}

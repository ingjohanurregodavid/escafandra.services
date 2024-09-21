using escafandra.services.Application.DTOs;
using escafandra.services.Application.Interfaces;
using escafandra.services.Domain.Entities;
using escafandra.services.Infrastructure.Factories;
using escafandra.services.Infrastructure.UnitOfWork;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace escafandra.services.Application.Services
{
    public class PdfService: IPdfService
    {
        private readonly IUnitOfWork _unitOfWork;

        public PdfService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<PdfResponseDto> UploadPdfAsync(PdfUploadDto uploadDto)
        {
            if(uploadDto == null || uploadDto.File == null)
            {
                throw new ArgumentNullException(nameof(uploadDto), "No file uploaded.");
            }

            // Convertir IFormFile a byte[] y guardar en la base de datos
            using var memoryStream = new MemoryStream();
            await uploadDto.File.CopyToAsync(memoryStream);
            var fileBytes = memoryStream.ToArray();

            var pdfDocument = new PDFDocument
            {
                FileName = uploadDto.File.FileName,
                FileData = fileBytes,
                UploadDate = DateTime.Now
            };

            await _unitOfWork.PdfRepository.AddAsync(pdfDocument);

            return new PdfResponseDto
            {
                Id = pdfDocument.Id,
                FileName = pdfDocument.FileName,
                UploadDate = pdfDocument.UploadDate
            };
        }

        public async Task<PdfResponseDto> DownloadPdfAsync(int id)
        {
            var pdfDocument = await _unitOfWork.PdfRepository.GetByIdAsync(id);
            if (pdfDocument == null) return null;

            return new PdfResponseDto { Id = pdfDocument.Id, FileName = pdfDocument.FileName, FileData = pdfDocument.FileData };
        }

        public async Task<PdfResponseDto> SignPdfAsync(int id)
        {
            var pdfDocument = await _unitOfWork.PdfRepository.GetByIdAsync(id);

            if (pdfDocument == null)
            {
                throw new KeyNotFoundException("PDF not found.");
            }
            // Asumiendo que el certificado se obtiene de alguna fuente segura
            var certificate = GetCertificate(); // Implementa la lógica para obtener el certificado
            var electronicSignatureFactory = new ElectronicSignatureFactory(certificate);
            var signature = await electronicSignatureFactory.CreateSignatureAsync(pdfDocument);

            pdfDocument.Signature = signature; 
            pdfDocument.IsSigned = true;
            pdfDocument.SignedDate = DateTime.UtcNow;

            await _unitOfWork.PdfRepository.UpdateAsync(pdfDocument);

            return new PdfResponseDto
            {
                Id = pdfDocument.Id,
                FileName = pdfDocument.FileName,
                IsSigned = pdfDocument.IsSigned
            };
        }

        public async Task<PdfResponseDto> GetPdfByIdAsync(int id)
        {
            var pdfDocument = await _unitOfWork.PdfRepository.GetByIdAsync(id);

            if (pdfDocument == null)
            {
                throw new KeyNotFoundException("PDF not found.");
            }

            var certificate = GetCertificate();
            var electronicSignatureFactory = new ElectronicSignatureFactory(certificate);
            var signature = await electronicSignatureFactory.CreateSignatureAsync(pdfDocument);

            pdfDocument.SignedBy = signature;
            pdfDocument.IsSigned = true;
            pdfDocument.SignedDate = DateTime.UtcNow;

            return new PdfResponseDto
            {
                Id = pdfDocument.Id,
                FileName = pdfDocument.FileName,
                FileData = pdfDocument.FileData
            };
        }
        public async Task<IEnumerable<PDFDocument>> GetAll()
        {
            
            var lstPDF = await _unitOfWork.PdfRepository.GetAllAsync();
            return lstPDF.ToList();
        }

        private X509Certificate2 GetCertificate()
        {
            var currentDirectory = Directory.GetCurrentDirectory();
            currentDirectory = Path.GetFullPath(Path.Combine(currentDirectory, ".."));
            var certificatePath = Path.Combine(currentDirectory, "escafandra.services.Shared", "certificates", "PFnubeAFCiudadano.crt");

            return new X509Certificate2(certificatePath);
        }
    }
}

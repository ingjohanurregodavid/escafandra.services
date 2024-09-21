using escafandra.services.API.Controllers;
using escafandra.services.Application.DTOs;
using escafandra.services.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using escafandra.services.Application.Services;
using escafandra.services.Infrastructure.UnitOfWork;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using escafandra.services.Domain.Entities;

namespace escafandra.services.Test.Controllers
{
    public class PdfControllerTests
    {
        private readonly Mock<IPdfService> _pdfServiceMock;
        private readonly PdfController _controller;

        public PdfControllerTests()
        {
            _pdfServiceMock = new Mock<IPdfService>();
            _controller = new PdfController(_pdfServiceMock.Object);
        }

        [Fact]
        public async Task Upload_ReturnsOkResult_WhenUploadIsSuccessful()
        {
            // Arrange
            var mockFile = new Mock<IFormFile>();
            var content = "Ingreso de prueba";
            var fileName = "test.pdf";
            var ms = new MemoryStream();
            var writer = new StreamWriter(ms);
            writer.Write(content);
            writer.Flush();
            ms.Position = 0;

            mockFile.Setup(_ => _.OpenReadStream()).Returns(ms);
            mockFile.Setup(_ => _.FileName).Returns(fileName);
            mockFile.Setup(_ => _.Length).Returns(ms.Length);

            var pdfUploadDto = new PdfUploadDto
            {
                File = mockFile.Object
            };

            // Configura el servicio mock para devolver un resultado esperado
            _pdfServiceMock.Setup(service => service.UploadPdfAsync(It.IsAny<PdfUploadDto>()))
                .ReturnsAsync(new PdfResponseDto { FileName = fileName });

            // Act
            var result = await _controller.Upload(pdfUploadDto);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<PdfResponseDto>(okResult.Value);
            Assert.Equal(fileName, returnValue.FileName);
        }

        [Fact]
        public async Task Download_ReturnsFileResult_WhenPdfExists()
        {
            // Arrange
            var id = 1;
            var mockPdf = new PdfResponseDto
            {
                FileName = "test.pdf",
                FileData = new byte[] { 1, 2, 3 }
            };

            _pdfServiceMock.Setup(service => service.GetPdfByIdAsync(id))
                           .ReturnsAsync(mockPdf);

            // Act
            var result = await _controller.Download(id);

            // Assert
            var fileResult = Assert.IsType<FileContentResult>(result);
            Assert.Equal("application/pdf", fileResult.ContentType);
            Assert.Equal(mockPdf.FileName, fileResult.FileDownloadName);
        }

        [Fact]
        public async Task GetAll_ReturnsOkResult_WithListOfPdfs()
        {
            // Arrange
            var mockPdfs = new List<PDFDocument>
            {
                new PDFDocument { FileName = "test1.pdf" },
                new PDFDocument { FileName = "test2.pdf" }
            };

            _pdfServiceMock.Setup(service => service.GetAll())
                           .ReturnsAsync(mockPdfs);

            // Act
            var result = await _controller.GetAll();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<List<PDFDocument>>(okResult.Value);
            Assert.Equal(2, returnValue.Count);
        }

        [Fact]
        public async Task Sign_ReturnsOkResult_WithSignedPdf()
        {
            // Arrange
            var id = 1;
            var signedPdf = new PdfResponseDto
            {
                FileName = "signed_test.pdf",
                IsSigned = true
            };

            _pdfServiceMock.Setup(service => service.SignPdfAsync(id))
                           .ReturnsAsync(signedPdf);

            // Act
            var result = await _controller.Sign(id);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<PdfResponseDto>(okResult.Value);
            Assert.True(returnValue.IsSigned);
            Assert.Equal(signedPdf.FileName, returnValue.FileName);
        }
    }
}

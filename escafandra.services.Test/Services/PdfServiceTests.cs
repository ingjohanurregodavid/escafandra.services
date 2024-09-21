using escafandra.services.Application.DTOs;
using escafandra.services.Application.Services;
using escafandra.services.Domain.Entities;
using escafandra.services.Infrastructure.Repositories.Interfaces;
using escafandra.services.Infrastructure.UnitOfWork;
using Microsoft.AspNetCore.Http;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace escafandra.services.Test.Services
{
    public class PdfServiceTests
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IPdfRepository> _pdfRepositoryMock;
        private readonly PdfService _pdfService;

        public PdfServiceTests()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _pdfRepositoryMock = new Mock<IPdfRepository>();

            _unitOfWorkMock.Setup(uow => uow.PdfRepository).Returns(_pdfRepositoryMock.Object);
            _pdfService = new PdfService(_unitOfWorkMock.Object);
        }
        [Fact]
        public async Task UploadPdfAsync_ValidFile_ReturnsPdfResponseDto()
        {
            // Arrange
            var uploadDto = new PdfUploadDto
            {
                File = new FormFile(new MemoryStream(new byte[] { 1, 2, 3 }), 0, 3, "file", "test.pdf")
            };

            var pdfDocument = new PDFDocument
            {
                Id = 1,
                FileName = "test.pdf",
                FileData = new byte[] { 1, 2, 3 },
                UploadDate = DateTime.Now
            };

            _pdfRepositoryMock.Setup(repo => repo.AddAsync(It.IsAny<PDFDocument>()))
                              .Callback<PDFDocument>(pdf => pdf.Id = 1)
                              .Returns(Task.CompletedTask);

            // Act
            var result = await _pdfService.UploadPdfAsync(uploadDto);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(1, result.Id);
            Assert.Equal("test.pdf", result.FileName);
            _pdfRepositoryMock.Verify(repo => repo.AddAsync(It.IsAny<PDFDocument>()), Times.Once);
        }

        [Fact]
        public async Task DownloadPdfAsync_ExistingId_ReturnsPdfResponseDto()
        {
            // Arrange
            var pdfDocument = new PDFDocument
            {
                Id = 1,
                FileName = "test.pdf",
                FileData = new byte[] { 1, 2, 3 }
            };

            _pdfRepositoryMock.Setup(repo => repo.GetByIdAsync(1))
                              .ReturnsAsync(pdfDocument);

            // Act
            var result = await _pdfService.DownloadPdfAsync(1);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(1, result.Id);
            Assert.Equal("test.pdf", result.FileName);
            _pdfRepositoryMock.Verify(repo => repo.GetByIdAsync(1), Times.Once);
        }

        [Fact]
        public async Task SignPdfAsync_ExistingPdf_ReturnsSignedPdfResponseDto()
        {
            // Arrange
            var pdfDocument = new PDFDocument
            {
                Id = 1,
                FileName = "test.pdf",
                IsSigned = false
            };

            _pdfRepositoryMock.Setup(repo => repo.GetByIdAsync(1))
                              .ReturnsAsync(pdfDocument);

            _pdfRepositoryMock.Setup(repo => repo.UpdateAsync(It.IsAny<PDFDocument>()))
                              .Returns(Task.CompletedTask);

            // Act
            var result = await _pdfService.SignPdfAsync(1);

            // Assert
            Assert.NotNull(result);
            Assert.True(result.IsSigned);
            _pdfRepositoryMock.Verify(repo => repo.GetByIdAsync(1), Times.Once);
            _pdfRepositoryMock.Verify(repo => repo.UpdateAsync(It.IsAny<PDFDocument>()), Times.Once);
        }

        [Fact]
        public async Task GetPdfByIdAsync_ExistingPdf_ReturnsPdfResponseDto()
        {
            // Arrange
            var pdfDocument = new PDFDocument
            {
                Id = 1,
                FileName = "test.pdf",
                FileData = new byte[] { 1, 2, 3 }
            };

            _pdfRepositoryMock.Setup(repo => repo.GetByIdAsync(1))
                              .ReturnsAsync(pdfDocument);

            // Act
            var result = await _pdfService.GetPdfByIdAsync(1);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(1, result.Id);
            Assert.Equal("test.pdf", result.FileName);
            _pdfRepositoryMock.Verify(repo => repo.GetByIdAsync(1), Times.Once);
        }

        [Fact]
        public async Task GetAll_ReturnsListOfPdfs()
        {
            // Arrange
            var pdfDocuments = new List<PDFDocument>
            {
                new PDFDocument { Id = 1, FileName = "test1.pdf" },
                new PDFDocument { Id = 2, FileName = "test2.pdf" }
            };

            _pdfRepositoryMock.Setup(repo => repo.GetAllAsync())
                              .ReturnsAsync(pdfDocuments);

            // Act
            var result = await _pdfService.GetAll();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count());
            _pdfRepositoryMock.Verify(repo => repo.GetAllAsync(), Times.Once);
        }

    }
}

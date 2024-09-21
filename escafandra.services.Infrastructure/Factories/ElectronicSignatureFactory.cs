using escafandra.services.Domain.Entities;
using escafandra.services.Infrastructure.Factories.Signatures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace escafandra.services.Infrastructure.Factories
{
    public class ElectronicSignatureFactory
    {
        private readonly X509Certificate2 _certificate;

        public ElectronicSignatureFactory(X509Certificate2 certificate)
        {
            _certificate = certificate;
        }

        public async Task<string> CreateSignatureAsync(PDFDocument pdfDocument)
        {
            // Obtén el contenido del PDF como un byte array
            byte[] pdfData = await GetPdfDataAsync(pdfDocument);

            var signatureGenerator = new CertificateBasedElectronicSignature(_certificate);
            byte[] signature = signatureGenerator.Sign(pdfData);

            return Convert.ToBase64String(signature);
        }

        private async Task<byte[]> GetPdfDataAsync(PDFDocument pdfDocument)
        {
            // Lógica para obtener el contenido del PDF (por ejemplo, desde un archivo o base de datos)
            return await Task.FromResult(pdfDocument.FileData); // Ejemplo
        }
    }
}

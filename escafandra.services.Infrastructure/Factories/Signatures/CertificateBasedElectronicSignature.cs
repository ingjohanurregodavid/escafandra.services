using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace escafandra.services.Infrastructure.Factories.Signatures
{
    public class CertificateBasedElectronicSignature
    {
        private readonly X509Certificate2 _certificate;
        public CertificateBasedElectronicSignature(X509Certificate2 certificate)
        {
            _certificate = certificate;
        }

        public byte[] Sign(byte[] dataToSign)
        {
            using (var rsa = _certificate.GetRSAPublicKey())
            {
                return rsa.SignData(dataToSign, HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);
            }
        }
    }
}

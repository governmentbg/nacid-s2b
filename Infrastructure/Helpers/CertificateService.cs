using Microsoft.AspNetCore.Hosting;
using System.Security.Cryptography.X509Certificates;

namespace Infrastructure.Helpers
{
    public class CertificateService
    {
        private readonly IHostingEnvironment _hostingEnvironment;

        public CertificateService(IHostingEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
        }

        public X509Certificate2 GetFromFile(string fileName, string certificatePass)
        {
            var location = Path.Combine(_hostingEnvironment.ContentRootPath, fileName);
            var content = File.ReadAllBytes(location);
            var cert = new X509Certificate2(content, certificatePass,
                X509KeyStorageFlags.MachineKeySet
                | X509KeyStorageFlags.PersistKeySet
                | X509KeyStorageFlags.Exportable);
            return cert;
        }
    }
}

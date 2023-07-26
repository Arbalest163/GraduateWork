using Microsoft.IdentityModel.Tokens;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;

namespace Chat.Persistence.Services
{
    internal static class SigningCertificate
    {
        private static readonly Lazy<SigningCredentials> Lazy = new Lazy<SigningCredentials>(Init);
        private const string passwordCertificate = "a50f504d0f104c25afe2ac23c1203285";

        private static SigningCredentials Init()
        {
            var certificate = LoadCertificate();
            var issuerSigningKey = new X509SecurityKey(certificate);
            return new SigningCredentials(issuerSigningKey, SecurityAlgorithms.RsaSha256);
        }

        public static SigningCredentials Get()
        {
            return Lazy.Value;
        }

        private static X509Certificate2 LoadCertificate()
        {
            var assembly = typeof(SigningCertificate).GetTypeInfo().Assembly;

            using var stream = assembly.GetManifestResourceStream(CertificateName);

            if (stream == null)
            {
                throw new NullReferenceException("Отсутствует сертификат для подписи.");
            }

            var buffer = new byte[stream.Length];
            stream.Read(buffer, 0, (int)stream.Length);


            return new X509Certificate2(buffer, passwordCertificate, X509KeyStorageFlags.MachineKeySet | X509KeyStorageFlags.PersistKeySet);
        }

        private const string CertificateName = "Chat.Persistence.Services.SignKey.pfx";
    }
}

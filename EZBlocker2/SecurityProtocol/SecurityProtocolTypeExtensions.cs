using System.Security.Authentication;

namespace System.Net
{
    public static class SecurityProtocolTypeExtensions
    {
        public const SecurityProtocolType Tls12 = (SecurityProtocolType)SslProtocolsExtensions.Tls12;
        public const SecurityProtocolType Tls11 = (SecurityProtocolType)SslProtocolsExtensions.Tls11;
        public const SecurityProtocolType SystemDefault = 0;
    }

}

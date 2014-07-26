using System;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using System.Threading;
using System.Threading.Tasks;
using Google.Apis.Analytics.v3;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Util.Store;

namespace GoogleServices
{
    internal static class Auth
    {
        /// <summary>
        /// Using standard Oauth Google WebPage
        /// </summary>
        /// <param name="clientId"></param>
        /// <param name="clientSecret"></param>
        /// <param name="user"></param>
        /// <param name="scopes"></param>
        /// <param name="store"></param>
        /// <param name="scopes"></param>
        /// <returns></returns>
        public static Task<UserCredential> AuthUsingWebPage(string clientId, string clientSecret, string user, string[] scopes, IDataStore store)
        {
            var credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
                         new ClientSecrets { ClientId = clientId, ClientSecret = clientSecret },
                         scopes,
                         user,
                         CancellationToken.None,
                        store);
            return credential;
        }

        /// <summary>
        /// Using service account
        /// </summary>
        /// <param name="p12Path"></param>
        /// <param name="email"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public static ServiceAccountCredential UsingServiceAccount(string p12Path, string email, string password)
        {
            if (!File.Exists(p12Path))
            {
                throw new FileNotFoundException(String.Format("File not found : {0}", p12Path), p12Path);
            }

            var certificate = new X509Certificate2(p12Path, password, X509KeyStorageFlags.Exportable);

            ServiceAccountCredential credential = new ServiceAccountCredential(
                new ServiceAccountCredential.Initializer(email)
                {
                    Scopes = new[] { AnalyticsService.Scope.Analytics }
                }.FromCertificate(certificate));
            return credential;
        }
    }
}

using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization.Formatters;
using Google.Apis.Analytics.v3;
using Google.Apis.Analytics.v3.Data;
using Google.Apis.Services;
using Google.Apis.Util.Store;

namespace GoogleServices
{
    public class Analytics
    {
        public string ApplicationName { get; private set; }

        private AnalyticsService service;

        /// <summary>
        /// Authentification using OAuth2 Web Page
        /// </summary>
        /// <param name="applicationName"></param>
        /// <param name="clientId"></param>
        /// <param name="clientSecret"></param>
        public Analytics(string applicationName, string clientId, string clientSecret)
        {
            ApplicationName = applicationName;
            var credential =
                Auth.AuthUsingWebPage(clientId, clientSecret, "user", new[] { AnalyticsService.Scope.AnalyticsReadonly }, new FileDataStore("Analytics.Auth.Store")).Result;

            service = new AnalyticsService(
                                   new BaseClientService.Initializer()
                                   {
                                       HttpClientInitializer = credential,
                                       ApplicationName = ApplicationName,
                                   });
        }

        /// <summary>
        /// Authtentification using service account
        /// </summary>
        /// <param name="applicationName"></param>
        /// <param name="p12Path"></param>
        /// <param name="email"></param>
        /// <param name="password"></param>
        public Analytics(string applicationName, string p12Path, string email, string password)
        {
            ApplicationName = applicationName;
            var credential = Auth.UsingServiceAccount(p12Path, email, password);

            // Create the service.
            service = new AnalyticsService(new BaseClientService.Initializer()
           {
               HttpClientInitializer = credential,
               ApplicationName = ApplicationName,
           });
        }

        /// <summary>
        /// Request data according to parameters
        /// </summary>
        /// <param name="requestParameters"></param>
        /// <returns></returns>
        public List<IList<string>> RequestData(RequestParameters requestParameters)
        {
            DataResource.GaResource.GetRequest request = service.Data.Ga.Get("ga:" + requestParameters.ProfileId,
                requestParameters.StartDate, requestParameters.EndDate,
                requestParameters.Metrics);
            request.Dimensions = requestParameters.Dimensions;
            request.QuotaUser = requestParameters.QuotaUser;
            request.MaxResults = requestParameters.MaxResults;
            request.Segment = requestParameters.Segment.Trim();
            if (request.Segment == "")
                request.Segment = null;
            request.Filters = requestParameters.Filters.Trim();
            if (request.Filters == "")
                request.Filters = null;

            int currentPosition = 1;
            List<IList<string>> results = new List<IList<string>>();
            int numberOfResults = 0;
            do
            {
                GaData d = request.Execute();  // Make the request
                if (d.Rows == null)
                    break;
                if (currentPosition == 1)
                {
                    results.Add(d.ColumnHeaders.Select(x => x.Name).ToList());
                }
                results.AddRange(d.Rows);
                numberOfResults = (!d.TotalResults.HasValue) ? 0 : d.TotalResults.Value;
                currentPosition += d.Rows.Count;
                request.StartIndex = currentPosition;
            } while (request.StartIndex <= numberOfResults);

            return results;
        }
    }
}

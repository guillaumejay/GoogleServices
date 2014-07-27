using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;


namespace GoogleServices.Console
{
    class Program
    {
        static int Main(string[] args)
        {
            bool verbose = false;
            string serviceEmail = ConfigurationManager.AppSettings["serviceEmail"];
            string password = ConfigurationManager.AppSettings["password"];
            string applicationName = ConfigurationManager.AppSettings["password"];
            string keyFile = ConfigurationManager.AppSettings["keyFile"]; ;
            Analytics myAnalytics = new Analytics(applicationName, keyFile, serviceEmail, password);
            RequestParameters rp = new RequestParameters();

            if (args.Any() && args.Contains("verbose"))
                verbose = true;
            FillUsingConsole(rp, verbose);
            List<IList<string>> results = myAnalytics.RequestData(rp);
            //using (StreamWriter sw = new StreamWriter("results.csv", false, Encoding.UTF8))
            //{
            foreach (IList<string> sublist in results)
            {
                string line = String.Join(";", sublist);
                //sw.WriteLine(line);
                //if (System.Diagnostics.Debugger.IsAttached)
                //{
                System.Console.WriteLine(line);
                //}
            }
            //}
            return 0;
        }

        private static void FillUsingConsole(RequestParameters rp, bool verbose)
        {
            if (verbose)
                System.Console.WriteLine("Profile ID :");
            string temp = System.Console.ReadLine();
            long profileId;
            if (!Int64.TryParse(temp, out profileId))
            {
                throw new ArgumentException("invalid ProfileId (check encoding) " + temp);
            }
            rp.ProfileId = profileId;
            if (verbose)
                System.Console.WriteLine("Metrics :");
            rp.Metrics = System.Console.ReadLine();
            if (verbose)
                System.Console.WriteLine("Dimensions :");
            rp.Dimensions = System.Console.ReadLine();
            if (verbose)
                System.Console.WriteLine("StartDate (YYYY-MM-DD) :");
            rp.StartDate = System.Console.ReadLine();
            if (verbose)
                System.Console.WriteLine("EndDate (YYYY-MM-DD) :");
            rp.EndDate = System.Console.ReadLine();
            if (verbose)
                System.Console.WriteLine("Filters :");
            rp.Filters = System.Console.ReadLine();
            if (verbose)
                System.Console.WriteLine("Segment :");
            rp.Segment = System.Console.ReadLine();
        }
    }
}

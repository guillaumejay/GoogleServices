using System;


namespace GoogleServices
{
    /// <summary>
    /// Parameters for Analytics request
    /// </summary>
    public class RequestParameters
    {
        public long ProfileId { get; set; }

        /// <summary>
        ///YYYY-MM-DD
        /// </summary>
        public string StartDate { get; set; }

        /// <summary>
        /// YYYY-MM-DD
        /// </summary>
        public string EndDate { get; set; }

        public string Dimensions { get; set; }

        public string Metrics { get; set; }

        public string QuotaUser { get; set; }

        public int? MaxResults { get; set; }

        public string Segment { get; set; }

        public string Filters { get; set; }
    }
}

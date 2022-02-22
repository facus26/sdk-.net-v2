using Decidir.Model.CyberSource;

namespace Decidir.Model
{
    public class ValidateData
    {
        public SiteInfo site { get; set; }
        public ValidateCustomer customer { get; set; }
        public ValidatePayment payment { get; set; }
        public string success_url { get; set; }
        public string redirect_url { get; set; }
        public string cancel_url { get; set; }
        public FraudDetection fraud_detection { get; set; }
    }
}
using Decidir.Exceptions;
using Decidir.Model.CyberSource;
using System;
using System.Collections.Generic;

namespace Decidir.Model
{
    public class Payment
    {
        public string site_transaction_id { get; set; }
        public string token { get; set; }
        public CustomerData customer { get; set; }
        public int payment_method_id { get; set; }
        public string bin { get; set; }
        public double amount { get; set; }
        public string currency { get; set; }
        public long installments { get; set; }
        public string description { get; set; }
        public string payment_type { get; set; }
        public string establishment_name { get; set; }
        public List<object> sub_payments { get; set; }
        public FraudDetection fraud_detection { get; set; }
        public string site_id { get; set; }
        public AggregateDataPayment aggregate_data { get; set; }
        public CardTokenBsa card_token_bsa { get; set; }

        public Payment()
        {
            this.sub_payments = new List<object>();
            this.customer = new CustomerData();
            this.site_id = null;
        }

        internal Payment copy() =>
            new Payment
            {
                site_transaction_id = this.site_transaction_id,
                token = this.token,
                customer = this.customer,
                payment_method_id = this.payment_method_id,
                bin = this.bin,
                amount = this.amount,
                currency = this.currency,
                installments = this.installments,
                description = this.description,
                payment_type = this.payment_type,
                establishment_name = this.establishment_name,
                fraud_detection = this.fraud_detection,
                site_id = this.site_id,
                aggregate_data = this.aggregate_data,
                sub_payments = this.sub_payments
            };

        public virtual void ConvertDecidirAmounts()
        {
            try
            {
                this.amount = Convert.ToInt64(this.amount * 100);

                foreach (object o in this.sub_payments)
                    ((SubPayment)o).amount = Convert.ToInt64(((SubPayment)o).amount * 100);

            }
            catch (Exception ex)
            {
                throw new ResponseException(ex.Message);
            }
        }
    }
}

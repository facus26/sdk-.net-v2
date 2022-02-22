using Decidir.Exceptions;
using System;

namespace Decidir.Model
{
    public class OfflinePayment : Payment
    {
        public string email { get; set; }
        public string invoice_expiration { get; set; }
        public string cod_p3 { get; set; }
        public string cod_p4 { get; set; }
        public string client { get; set; }
        public double? surcharge { get; set; }
        public string second_invoice_expiration { get; set; }
        public string payment_mode { get; set; }
        public int? bank_id { get; set; }

        internal OfflinePayment copyOffline() =>
            new OfflinePayment
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
                fraud_detection = this.fraud_detection,
                site_id = this.site_id,

                email = this.email,
                invoice_expiration = this.invoice_expiration,
                cod_p3 = this.cod_p3,
                cod_p4 = this.cod_p4,
                client = this.client,
                payment_mode = this.payment_mode,
                second_invoice_expiration = this.second_invoice_expiration,
                surcharge = this.surcharge,
                bank_id = this.bank_id,
                sub_payments = this.sub_payments
            };

        public override void ConvertDecidirAmounts()
        {
            try
            {
                this.amount = Convert.ToInt64(this.amount * 100);

                if (this.surcharge != null)
                    this.surcharge = Convert.ToInt64((this.surcharge) * 100);

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

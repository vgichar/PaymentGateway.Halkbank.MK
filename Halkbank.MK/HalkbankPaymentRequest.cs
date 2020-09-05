using System;

namespace Halkbank.MK
{
    public class HalkbankPaymentRequest
    {
        /// <summary>
        /// CTOR
        /// </summary>
        /// <param name="settings"></param>
        public HalkbankPaymentRequest(HalkbankSettings settings)
        {
            if (settings == null)
                throw new ArgumentNullException(nameof(settings));

            Storetype = "3d_pay_hosting";
            Refreshtime = 5;
            Encoding = "utf-8";
            Trantype = "Auth";
            Currency = "807";
            Lang = "mk";

            StoreKey = settings.StoreKey;
            Clientid = settings.Clientid;
            PortalUrl = settings.PortalUrl;
            Rnd = Helpers.GenerateRandom(6);
        }

        /// <summary>
        /// Halkbank store key, provided by the bank
        /// </summary>
        public string StoreKey { get; set; }

        /// <summary>
        /// Halkbank client id, provided by the bank
        /// </summary>
        public string Clientid { get; set; }

        /// <summary>
        /// Order amount
        /// </summary>
        public decimal Amount { get; set; }
        private string amountString => Amount.ToString("F2");

        /// <summary>
        /// Order id
        /// </summary>
        public string Oid { get; set; }

        /// <summary>
        /// Redirect URL on success
        /// </summary>
        public string OkUrl { get; set; }

        /// <summary>
        /// Redirect URL on fail
        /// </summary>
        public string FailUrl { get; set; }

        /// <summary>
        /// Server to server callback URL after success
        /// </summary>
        public string CallbackUrl { get; set; }

        /// <summary>
        /// Environment payment URL
        /// </summary>
        public string PortalUrl { get; set; }

        /// <summary>
        /// Default: 3d_pay_hosting
        /// </summary>
        public string Storetype { get; set; }

        /// <summary>
        /// Default: Auth
        /// </summary>
        public string Trantype { get; set; }
        
        /// <summary>
        /// Default: Auth
        /// </summary>
        public int? Instalment { get; set; }

        /// <summary>
        /// Default: 807
        /// </summary>
        public string Currency { get; set; }

        /// <summary>
        /// Default: mk
        /// </summary>
        public string Lang { get; set; }

        /// <summary>
        /// Default: utf-8
        /// </summary>
        public string Encoding { get; set; }

        /// <summary>
        /// Default: 5
        /// </summary>
        public int Refreshtime { get; set; }

        /// <summary>
        /// Random string
        /// </summary>
        public string Rnd { get; set; }

        /// <summary>
        /// Generated hash from: $"{Clientid}{Oid}{Amount}{OkUrl}{FailUrl}{Trantype}{Rnd}{CallbackUrl}{StoreKey}"
        /// </summary>
        public string Hash
        {
            get
            {
                var concat = $"{Clientid}{Oid}{amountString}{OkUrl}{FailUrl}{Trantype}{Instalment}{Rnd}{CallbackUrl}{StoreKey}";
                var encoded = Helpers.Encode(concat, Helpers.HalkbankEncoding);
                var hashed = Helpers.ToSha1(encoded);
                var base64 = Helpers.ToBase64String(hashed);

                return base64;
            }
        }

        /// <summary>
        /// Generate HTML form to render
        /// </summary>
        /// <param name="customHtmlInsideForm">
        /// Custom HTML to render inside the form, example: 
        /// &lt;button type="submit"&gt;Pay now&lt;/button&gt;
        /// </param>
        /// <returns></returns>
        public string ToHtmlForm(string customHtmlInsideForm)
        {
            return $@"
<form method=""post"" action=""{PortalUrl}"">
<input type=""hidden"" name=""clientid"" value=""{Clientid}"" />
<input type=""hidden"" name=""storetype"" value=""{Storetype}"" />
<input type=""hidden"" name=""trantype"" value=""{Trantype}"" />
<input type=""hidden"" name=""instalment"" value=""{Instalment}"" />
<input type=""hidden"" name=""amount"" value=""{amountString}"" />
<input type=""hidden"" name=""currency"" value=""{Currency}"" />
<input type=""hidden"" name=""oid"" value=""{Oid}"" />
<input type=""hidden"" name=""okUrl"" value=""{OkUrl}"" />
<input type=""hidden"" name=""failUrl"" value=""{FailUrl}"" />
<input type=""hidden"" name=""callbackUrl"" value=""{CallbackUrl}"" />
<input type=""hidden"" name=""lang"" value=""{Lang}"" />
<input type=""hidden"" name=""encoding"" value=""{Encoding}"" />
<input type=""hidden"" name=""refreshtime"" value=""{Refreshtime}"" />
<input type=""hidden"" name=""rnd"" value=""{Rnd}"" />
<input type=""hidden"" name=""hash"" value=""{Hash}"" />
{customHtmlInsideForm}
</form>";
        }
    }
}

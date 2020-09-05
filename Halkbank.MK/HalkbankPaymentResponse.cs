using System;
using System.Collections.Generic;
using System.Linq;

namespace Halkbank.MK
{
    public class HalkbankPaymentResponse
    {
        /// <summary>
        /// Response "Approved" or "Not Approved"
        /// </summary>
        public string Response { get; }

        /// <summary>
        /// Authorization code of the transaction
        /// </summary>
        public string AuthCode { get; }

        /// <summary>
        /// Host reference number
        /// </summary>
        public string HostRefNum { get; }

        /// <summary>
        /// Transaction status code
        /// </summary>

        public string ProcReturnCode { get; }

        /// <summary>
        /// Unique transaction ID
        /// </summary>
        public string TransId { get; }

        /// <summary>
        /// Error text (if Response “Declined” or “Error” )
        /// </summary>
        public string ErrMsg { get; }

        /// <summary>
        /// IP address of the customer
        /// </summary>
        public string ClientIp { get; }

        /// <summary>
        /// Returned order ID, must be same as input oid
        /// </summary>
        public string ReturnOid { get; }

        /// <summary>
        /// Masked credit card number
        /// </summary>
        public string MaskedPan { get; }

        /// <summary>
        /// Payment method of the transaction
        /// </summary>
        public string PaymentMethod { get; }

        /// <summary>
        /// Random string, will be used for hash comparison
        /// </summary>
        public string Rnd { get; }

        /// <summary>
        /// Contains the field names used for hash calculation. Field names are appended with ":" character
        /// </summary>
        public string Hashparams { get; }

        /// <summary>
        /// Contains the appended hash field values for hash calculation. Field values appended with the same order in HASHPARAMS field
        /// </summary>
        public string Hashparamsva { get; }

        /// <summary>
        /// Hash value of HASHPARAMSVAL and merchant password field
        /// </summary>
        public string Hash { get; }

        /// <summary>
        /// 3D secure transaction status code
        /// </summary>
        public string MdStatus { get; }

        /// <summary>
        /// 3D status for archival
        /// </summary>
        public string Txstatus { get; }

        /// <summary>
        /// Electronic Commerce Indicator
        /// </summary>
        public string Eci { get; }

        /// <summary>
        /// Cardholder Authentication Verification Value, determined by ACS.
        /// </summary>
        public string Cavv { get; }

        /// <summary>
        /// Error Message from MPI (if any)
        /// </summary>
        public string MdErrorMsg { get; }

        /// <summary>
        /// Unique Internet transaction ID
        /// </summary>
        public string Xid { get; }

        /// <summary>
        /// Is success
        /// </summary>
        public bool Success { get; }

        /// <summary>
        /// CTOR
        /// </summary>
        /// <param name="settings"></param>
        /// <param name="requestBodyStream"></param>
        /// <returns></returns>
        public HalkbankPaymentResponse(HalkbankSettings settings, string requestBodyContent)
        {
            if (settings == null)
                throw new ArgumentNullException(nameof(settings));

            if (string.IsNullOrEmpty(requestBodyContent))
                throw new ArgumentNullException(nameof(requestBodyContent));

            var parsed = requestBodyContent?
                .Split('?')?
                .LastOrDefault()?
                .Split('&')?
                .Select(pair =>
                {
                    var segments = pair.Split('=');

                    if (segments.Length < 2)
                        return new KeyValuePair<string, string>(segments.FirstOrDefault(), null);

                    return new KeyValuePair<string, string>(segments.FirstOrDefault(), segments.LastOrDefault());
                })?
                .ToDictionary(x => x.Key, x => x.Value)
                ?? new Dictionary<string, string>();

            parsed.TryGetValue("Response", out var response);
            parsed.TryGetValue("AuthCode", out var authCode);
            parsed.TryGetValue("HostRefNum", out var hostRefNum);
            parsed.TryGetValue("ProcReturnCode", out var procReturnCode);
            parsed.TryGetValue("TransId", out var transId);
            parsed.TryGetValue("ErrMsg", out var errMsg);
            parsed.TryGetValue("ClientIp", out var clientIp);
            parsed.TryGetValue("ReturnOid", out var returnOid);
            parsed.TryGetValue("MaskedPan", out var maskedPan);
            parsed.TryGetValue("PaymentMethod", out var paymentMethod);
            parsed.TryGetValue("rnd", out var rnd);
            parsed.TryGetValue("HASHPARAMS", out var hashparams);
            parsed.TryGetValue("HASHPARAMSVAL", out var hashparamsva);
            parsed.TryGetValue("HASH", out var hash);
            parsed.TryGetValue("mdStatus", out var mdStatus);
            parsed.TryGetValue("txstatus", out var txstatus);
            parsed.TryGetValue("eci", out var eci);
            parsed.TryGetValue("cavv", out var cavv);
            parsed.TryGetValue("mdErrorMsg", out var mdErrorMsg);
            parsed.TryGetValue("xid", out var xid);

            Response = response;
            AuthCode = authCode;
            HostRefNum = hostRefNum;
            ProcReturnCode = procReturnCode;
            TransId = transId;
            ErrMsg = errMsg;
            ClientIp = clientIp;
            ReturnOid = returnOid;
            MaskedPan = maskedPan;
            PaymentMethod = paymentMethod;
            Rnd = rnd;
            Hashparams = hashparams;
            Hashparamsva = hashparamsva;
            Hash = hash;
            MdStatus = mdStatus;
            Txstatus = txstatus;
            Eci = eci;
            Cavv = cavv;
            MdErrorMsg = mdErrorMsg;
            Xid = xid;

            var concat = $"{Hashparamsva}{settings.StoreKey}";
            var encoded = Helpers.Encode(concat, Helpers.HalkbankEncoding);
            var hashed = Helpers.ToSha1(encoded);
            var base64 = Helpers.ToBase64String(hashed);

            Success = Response == "Approved" && base64 == Hash;
        }
    }
}

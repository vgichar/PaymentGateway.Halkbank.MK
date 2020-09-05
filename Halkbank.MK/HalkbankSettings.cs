using System;
using System.Collections.Generic;
using System.Text;

namespace Halkbank.MK
{
    public class HalkbankSettings
    {
        /// <summary>
        /// CTOR
        /// </summary>
        /// <param name="clientId"></param>
        /// <param name="storeKey"></param>
        /// <param name="portalUrl"></param>
        public HalkbankSettings(string clientId, string storeKey, string portalUrl)
        {
            if (string.IsNullOrEmpty(clientId))
                throw new ArgumentNullException(nameof(clientId));
            
            if (string.IsNullOrEmpty(storeKey))
                throw new ArgumentNullException(nameof(storeKey));

            Clientid = clientId;
            StoreKey = storeKey;
            PortalUrl = portalUrl;
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
        /// Payment portal url
        /// </summary>
        public string PortalUrl { get; set; }
    }
}

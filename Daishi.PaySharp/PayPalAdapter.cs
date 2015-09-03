#region Includes

using System;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;

#endregion

namespace Daishi.PaySharp {
    /// <summary>
    ///     PayPalAdapter interfaces with PayPal HTTP endpoints and provides both
    ///     synchronous and asynchronous mechanisms that consume those endpoints.
    ///     <remarks>
    ///         PayPal exposes metadata in a form-urlencoded PayPalAdapter
    ///         provides a means to retrieve PayPal metadata in raw-format.
    ///     </remarks>
    /// </summary>
    public class PayPalAdapter {

        /// <summary>
        ///     Executes PayPal's SetExpressCheckout function in order to return a
        ///     PayPal Access Token.
        /// </summary>
        /// <param name="payload">
        ///     Metadata necessary to facilitate a successful SetExpressCheckout call.
        ///     Payload will be converted to key-value format.
        /// </param>
        /// <param name="encoding">Text encoding to apply during byte-to-text conversion.</param>
        /// <param name="expressCheckoutURI">Default PayPal ExpressCheckout HTTP URI.</param>
        /// <returns>Raw metadata, in key-value format, containing a PayPal Access Token.</returns>
        public string SetExpressCheckout(SetExpressCheckoutPayload payload,
            Encoding encoding, string expressCheckoutURI) {

            var nvc = new NameValueCollection {
                {"USER", payload.User},
                {"PWD", payload.Password},
                {"SIGNATURE", payload.Signature},
                {"METHOD", payload.Method},
                {"VERSION", payload.Version},
                {"PAYMENTREQUEST_0_PAYMENTACTION", payload.Action},
                {"PAYMENTREQUEST_0_AMT", payload.Amount},
                {"PAYMENTREQUEST_0_CURRENCYCODE", payload.CurrencyCode},
                {"cancelUrl", payload.CancelUrl},
                {"returnUrl", payload.ReturnUrl}
            };

            using (var webClient = new WebClient()) {

                var response = webClient.UploadValues(expressCheckoutURI, nvc);
                return encoding.GetString(response);
            }
        }

        /// <summary>
        ///     SetExpressCheckout asynchronous equivalent.
        ///     <seealso cref="SetExpressCheckout" />
        /// </summary>
        public async Task<string> SetExpressCheckoutAsync(SetExpressCheckoutPayload payload,
            Encoding encoding, string expressCheckoutURI) {

            var nvc = new NameValueCollection {
                {"USER", payload.User},
                {"PWD", payload.Password},
                {"SIGNATURE", payload.Signature},
                {"METHOD", payload.Method},
                {"VERSION", payload.Version},
                {"PAYMENTREQUEST_0_PAYMENTACTION", payload.Action},
                {"PAYMENTREQUEST_0_AMT", payload.Amount},
                {"PAYMENTREQUEST_0_CURRENCYCODE", payload.CurrencyCode},
                {"cancelUrl", payload.CancelUrl},
                {"returnUrl", payload.ReturnUrl}
            };

            using (var webClient = new WebClient()) {

                var response = await webClient.UploadValuesTaskAsync(expressCheckoutURI, nvc);
                return encoding.GetString(response);
            }
        }

        /// <summary>
        ///     Executes PayPal's GetExpressCheckoutDetails function in order to
        ///     return PayPal Customer Details.
        /// </summary>
        /// <param name="payload">
        ///     Metadata necessary to facilitate a successful GetExpressCheckoutDetails call.
        ///     Payload will be converted to key-value format.
        /// </param>
        /// <param name="expressCheckoutUri">Default PayPal ExpressCheckout HTTP URI.</param>
        /// <returns>Raw metadata, in key-value format, containing PayPal Customer Details.</returns>
        public string GetExpressCheckoutDetails(
            GetExpressCheckoutDetailsPayload payload, string expressCheckoutUri) {

            var nvc = new NameValueCollection {
                {"USER", payload.User},
                {"PWD", payload.Password},
                {"SIGNATURE", payload.Signature},
                {"METHOD", payload.Method},
                {"VERSION", payload.Version},
                {"TOKEN", payload.AccessToken}
            };

            var queryString = string.Join("&", nvc.AllKeys.Select(
                i => string.Concat(i, "=", HttpUtility.UrlEncode(nvc[i]))));

            using (var webClient = new WebClient()) {

                return webClient.DownloadString(
                    new Uri(string.Concat(expressCheckoutUri, "?", queryString)));
            }
        }

        /// <summary>
        ///     GetExpressCheckoutDetails asynchronous equivalent.
        ///     <seealso cref="GetExpressCheckoutDetails" />
        /// </summary>
        public async Task<string> GetExpressCheckoutDetailsAsync(
            GetExpressCheckoutDetailsPayload payload, string getExpressCheckoutUri) {

            var nvc = new NameValueCollection {
                {"USER", payload.User},
                {"PWD", payload.Password},
                {"SIGNATURE", payload.Signature},
                {"METHOD", payload.Method},
                {"VERSION", payload.Version},
                {"TOKEN", payload.AccessToken}
            };

            var queryString = string.Join("&", nvc.AllKeys.Select(
                i => string.Concat(i, "=", HttpUtility.UrlEncode(nvc[i]))));

            using (var webClient = new WebClient()) {

                return await webClient.DownloadStringTaskAsync(
                    new Uri(string.Concat(getExpressCheckoutUri, "?", queryString)));
            }
        }
    }
}
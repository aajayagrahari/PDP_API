using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Hosting;
using System.Xml.Linq;

namespace PowerDesignPro.Common.MessageConfig
{
    /// <summary>
    /// Class MessageCollection.
    /// </summary>
    public class MessageCollection
    {
        /// <summary>
        /// MessageCollection private consrtuctor
        /// </summary>
        private MessageCollection()
        {
            //var path1 = GetBaseUrl();
            //var path2 = ConfigurationManager.AppSettings["MessageConfigFile"];
            //MessageConfigFile = System.IO.Path.Combine(path1, path2);
            MessageConfigFile = HostingEnvironment.MapPath("~/App_Data/ErrorMessage.xml");
            MessageElements = XElement.Load(MessageConfigFile);
        }

        /// <summary>
        /// The instance
        /// </summary>
        private static MessageCollection _instance;

        /// <summary>
        /// MessageCollection Lazy Instantiation
        /// </summary>
        /// <value>The instance.</value>
        public static MessageCollection Instance => _instance ?? (_instance = new MessageCollection());

        private string MessageConfigFile { get; set; }

        private XElement MessageElements { get; set; }

        /// <summary>
        /// Get message for code.
        /// </summary>
        public string GetMessage(string code, string categoryName)
        {
            //Retrive the Module Element
            var moduleElement = from msg in MessageElements.Elements("Module")
                where (string)msg.Attribute("category") == categoryName
                select msg;

            //Retrive the Message Element
            var validationElement = from msg in moduleElement.Elements("Validation")
                where (string)msg.Attribute("code") == code
                select msg;

            var messageValue = validationElement.FirstOrDefault();

            return messageValue?.Value ?? string.Empty;
        }

        /// <summary>
        /// Gets the base URL.
        /// </summary>
        private static string GetBaseUrl()
        {
            var request = HttpContext.Current.Request;
            var appUrl = HttpRuntime.AppDomainAppVirtualPath;

            if (appUrl != "/")
            {
                appUrl = "/" + appUrl;
            }

            var baseUrl = $"{request.Url.Scheme}://{request.Url.Authority}{appUrl}";

            return baseUrl;
        }
    }
}

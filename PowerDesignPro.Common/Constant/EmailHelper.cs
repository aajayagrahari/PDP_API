using Newtonsoft.Json;
using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using static SendGrid.SendGridClient;

namespace PowerDesignPro.Common
{
    public static class EmailHelper
    {
        private static string PowerDesignProEmailAPIKey = ConfigurationManager.AppSettings["PowerDesignProEmailAPIKey"].ToString();

        // Use NuGet to install SendGrid (Basic C# client lib) 
        public static async Task ConfigSendGridasync(MailMessage message)
        {
            #region formatter
            //string text = string.Format("Please click on this link to {0}: {1}", message.Subject, message.Body);
            string html = message.Body;

            //html += HttpUtility.HtmlEncode(@"Or click on the copy the following link on the browser:" + message.Body);
            #endregion

            SmtpClient smtpClient = new SmtpClient();
            //System.Net.NetworkCredential credentials = new System.Net.NetworkCredential("joe@contoso.com", "XXXXXX");
            //smtpClient.Credentials = credentials;
            //smtpClient.EnableSsl = true;
            await smtpClient.SendMailAsync(message);
        }

        public static async Task<HttpStatusCode> SendGridasync(MailMessage message)
        {
            var apiKey = PowerDesignProEmailAPIKey;
            var client = new SendGridClient(apiKey);
            var from = new EmailAddress(message.From.ToString(), "Test From");
            var subject = message.Subject;
            var to = new EmailAddress(message.To.ToString(), "Test To User");
            var plainTextContent = message.Body;
            var htmlContent = message.Body;
            var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);
            //msg.TemplateId = "d0a27c84-1dfd-4e05-a358-6defa4238ff9";
            //msg.Sections = new Dictionary<string, string>();
            //msg.Sections.Add(":userName", "pradeep.y@test.com");
            //msg.Sections.Add(":notes", "SDcvyasiduhfvaudsihfi");
            //msg.Sections.Add(":url", "< a href =\"" + "http://localhost:50766/project/20" + "\">here</a>.");
            //msg.AddSubstitution("-userName-", "pradeep.y@test.com");
            //msg.AddSubstitution("-notes-", "SDcvyasiduhfvaudsihfi");
            //msg.AddSubstitution("-url-", "<a href =\"" + "http://localhost:50766/project/20" + "\">here</a>.");
            var response = await client.SendEmailAsync(msg);
            return response.StatusCode;
        }

        public static async Task<HttpStatusCode> SendGridAsyncWithTemplate(SendGridEmailData emailData)
        {
            var apiKey = PowerDesignProEmailAPIKey;
            var client = new SendGridClient(apiKey);

            string data = JsonConvert.SerializeObject(emailData);

            var json = JsonConvert.DeserializeObject<Object>(data);
            var response = await client.RequestAsync(SendGridClient.Method.POST, json.ToString(), urlPath: "mail/send");
            return response.StatusCode;
        }

        public static string CompanyName(string language, string urlBrand)
        {
            var companyName = SectionHandler.GetSectionValue($"{language.ToLower()}/emailTemplates/companyName", urlBrand.ToLower(), "");
            return companyName;
        }

        public static string CompanyAddress(string language, string urlBrand)
        {
            var companyName = SectionHandler.GetSectionValue($"{language.ToLower()}/emailTemplates/companyAddress", urlBrand.ToLower(), "");
            return companyName;
        }
    }
}

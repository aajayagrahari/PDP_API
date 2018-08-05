using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PowerDesignPro.Common
{    
    public class Personalization
    {
        public Personalization()
        {
            to = new List<To>();
            substitutions = new Dictionary<string, string>();
        }

        public List<To> to { get; set; }

        public Dictionary<string, string> substitutions { get; set; }
    }

    public class To
    {
        public string email { get; set; }

        public string name { get; set; }
    }

    public class From
    {
        public string email { get; set; } = "noreply@powerdesignpro.com";

        public string name { get; set; } = "Power Design Pro";
    }

    public class SendGridEmailData
    {
        public SendGridEmailData()
        {
            personalizations = new List<Personalization>();
            from = new From();
        }

        public List<Personalization> personalizations { get; set; }

        public From from { get; set; }

        public string template_id { get; set; }
    }
}

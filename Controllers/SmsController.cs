using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Vonage.Messaging;
using Vonage.Request;

namespace SendSmsAspDotnetMvc.Controllers
{
    public class SmsController : Controller
    {
        public IConfiguration Configuration { get; set; }

        public SmsController(IConfiguration config)
        {
            Configuration = config;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Sms(Models.SmsModel sendSmsModel)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var VONAGE_API_KEY = Configuration["VONAGE_API_KEY"];
                    var VONAGE_API_SECRET = Configuration["VONAGE_API_SECRET"];
                    var credentials = Credentials.FromApiKeyAndSecret(VONAGE_API_KEY, VONAGE_API_SECRET);
                    var client = new SmsClient(credentials);
                    var request = new SendSmsRequest { To = sendSmsModel.To, From = sendSmsModel.From, Text = sendSmsModel.Text };
                    var response = client.SendAnSms(request);
                    ViewBag.MessageId = response.Messages[0].MessageId;
                }
                catch(VonageSmsResponseException ex)
                {
                    ViewBag.Error = ex.Message;
                }
                
            }
            return View("Index");
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNet.Identity;

namespace TeachMeBackendService.Logic
{
    public class EmailService : IIdentityMessageService
    {
        public Task SendAsync(IdentityMessage message)
        {
            const string apiKey = "key-555d8dd6f0a49c757f93ea6edd32824d";
            const string sandBox = "sandbox0e3961d47dcb414c8554f20b83297606.mailgun.org";
            byte[] apiKeyAuth = Encoding.ASCII.GetBytes($"api:{apiKey}");
            var httpClient = new HttpClient { BaseAddress = new Uri("https://api.mailgun.net/v3/") };
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic",
                Convert.ToBase64String(apiKeyAuth));

            var form = new Dictionary<string, string>
            {
                ["from"] = "Mailgun Sandbox <postmaster@sandbox0e3961d47dcb414c8554f20b83297606.mailgun.org>",
                ["to"] = message.Destination,
                ["subject"] = message.Subject,
                ["text"] = message.Body
            };

            HttpResponseMessage response =
                httpClient.PostAsync(sandBox + "/messages", new FormUrlEncodedContent(form)).Result;
            return Task.FromResult((int)response.StatusCode);
        }
    }
}
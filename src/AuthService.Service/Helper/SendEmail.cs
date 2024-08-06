using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using AuthService.Domain.DTOs.Request;

namespace AuthService.Service.Helper
{

    public static class SendEmail
    {
        
        public static async Task<bool> SendConfirmationEmail(IHttpClientFactory _httpClientFactory,SendEmailConfirmation request)
        {// create a object for email confirmation


            var httpclient = _httpClientFactory.CreateClient();
            var emailModel = new
            {
                To = request.UserEmail,
                Subject = request.Subject,
                Body = $"Hello {request.FirstName}, here is ur verification token: {request.Token}"

            };
            var sendEmail = await httpclient.PostAsJsonAsync("https://localhost:7168/api/Notification", emailModel);

            // Check if the email was sent successfully
            if (sendEmail.IsSuccessStatusCode)
            {
                // Create the verification token object

                return true;
            }

            // Handle the error appropriately (log, throw exception, etc.)
            return false;



        }
    }
}

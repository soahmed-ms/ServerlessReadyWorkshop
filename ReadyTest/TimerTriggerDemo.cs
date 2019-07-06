using System;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace ReadyTest
{
    public static class TimerTriggerDemo
    {
        [FunctionName("TimerTriggerDemo")]
        public static async System.Threading.Tasks.Task RunAsync([TimerTrigger("0 */1 * * * *")]TimerInfo myTimer, ILogger log)
        {
            log.LogInformation($"C# Timer trigger function executed at: {DateTime.Now}");
            var client = new SendGridClient("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");
            var from = new EmailAddress("ansary@microsoft.com", "Anshita");
            var subject = "Your coffee is Ready!";
            var to = new EmailAddress("soahmed@microsoft.com", "Sofia");
            var plainTextContent = "Serverless! Do more!";
            var htmlContent = "<strong>Do more, with Serverless.</strong>";
            var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);
            var response = await client.SendEmailAsync(msg).ConfigureAwait(false);
        }
    }
}

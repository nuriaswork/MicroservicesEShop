using Microsoft.EntityFrameworkCore;
using MsEShop.Services.EmailAPI.Data;
using MsEShop.Services.EmailAPI.Models;
using MsEShop.Services.EmailAPI.Models.Dto;
using System.Text;

namespace MsEShop.Services.EmailAPI.Services
{
    public class EmailService : IEmailService
    {
        //        //we cannot use AddDbContext with independency injection in a Singleton instance, so we have to add a DbContextOptionsBuilder in Program.cs
        //        var optionBuilder = new DbContextOptionsBuilder<AppDbContext>();
        //        optionBuilder.UseMySQL(builder.Configuration.GetConnectionString("MySQLConnection")!);
        //        builder.Services.AddSingleton(new EmailService(optionBuilder.Options));

        //This is generated because we set builder.Services.AddSingleton(new EmailService(optionBuilder.Options)); in Program.cs
        private DbContextOptions<AppDbContext> _options;
        public EmailService(DbContextOptions<AppDbContext> options)
        {
            _options = options;
        }

        public async Task EmailCartAndLog(EmailCartDto emailCartDto)
        {
            //build a simple email body
            StringBuilder mailBody = new StringBuilder();
            mailBody.AppendLine("<p>This is your cart:</p>");
            mailBody.AppendLine("<p>Total:</p>" + emailCartDto.CartHeader?.CartTotal);
            mailBody.AppendLine("<ul>");
            foreach (var item in emailCartDto.CartDetails)
            {
                mailBody.AppendLine("<li>" + item.productDto.Name + " x " + item.Count + "</li>");
            }
            mailBody.AppendLine("</ul>");

            await LogAndEmail(mailBody.ToString(), emailCartDto.UserEmail);
        }

        private async Task<bool> LogAndEmail(string message, string emailTo)
        {
            try
            {
                EmailLogger emailLog = new()
                {
                    Email = emailTo,
                    Message = message,
                    EmailSentOn = DateTime.Now
                };

                await using var _db = new AppDbContext(_options);
                _db.EmailLoggers.Add(emailLog);
                await _db.SaveChangesAsync();
                return true;

            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}

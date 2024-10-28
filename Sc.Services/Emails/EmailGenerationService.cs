using HandlebarsDotNet;
using Infrastructure.AppSettings;
using Infrastructure.Constants;
using Microsoft.EntityFrameworkCore;
using Sc.Models;
using Sc.Models.Dtos.Auth;
using Sc.Models.Entities.Emails;
using Sc.Models.Enums.Suppliers;

namespace Sc.Services.Emails
{
    public class EmailGenerationService
    {
        private readonly ScDbContext context;

        public EmailGenerationService(ScDbContext context)
        {
            this.context = context;
        }

        public async Task SupplierRegistrationComposeEmail(SignUpDto signUpDto)
        {
            if (AppSettingsProvider.EmailConfiguration.JobEnabled && AppSettingsProvider.EmailConfiguration.SupplierRegistrationInfoMailsTo.Any())
            {
                var supplierName = signUpDto.SupplierType == SupplierType.Complex
                ? signUpDto.Complex.Name
                : $"{signUpDto.Institution.Name} {(signUpDto.Institution.Id != signUpDto.RootInstitution.Id ? $"({signUpDto.RootInstitution.ShortName})" : string.Empty)}";

                var templateData = new
                {
                    SupplierName = supplierName,
                    signUpDto.User.UserName,
                    signUpDto.User.UserInfo.FullName
                };

                await ComposeEmail(EmailTemplateAliases.SupplierRegistration, null, templateData, AppSettingsProvider.EmailConfiguration.SupplierRegistrationInfoMailsTo);
            }
        }

        private async Task ComposeEmail(string alias, object subjectData, object templateData, List<string> recipients)
        {
            if (AppSettingsProvider.EmailConfiguration.JobEnabled)
            {
                var emailTemplate = await context.EmailTemplates
                    .AsNoTracking()
                    .SingleAsync(e => e.Alias == alias);

                string body = emailTemplate.BodyTemplate;
                string subject = emailTemplate.SubjectTemplate;

                if (subjectData != null)
                {
                    var subjectTemplate = Handlebars.Compile(emailTemplate.SubjectTemplate);
                    subject = subjectTemplate(subjectData);
                }

                if (templateData != null)
                {
                    var template = Handlebars.Compile(emailTemplate.BodyTemplate);
                    body = template(templateData);
                }

                foreach (var recipient in recipients)
                {
                    var email = new Email(subject, body, recipient);

                    context.Emails.Add(email);
                    await context.SaveChangesAsync();
                }
            }
        }
    }
}

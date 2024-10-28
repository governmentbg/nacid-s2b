using Microsoft.EntityFrameworkCore;
using Sc.Models;
using Sc.Models.Entities.Nomenclatures.Institutions;
using Sc.Models.Enums.Common;
using Sc.Models.Enums.Institutions;
using Sc.Repositories.Helpers;

namespace MessageBroker.Consumer.Services
{
    public class RndOrganizationUpdateService
    {
        private readonly ScDbContext context;

        public RndOrganizationUpdateService(
            ScDbContext context
            )
        {
            this.context = context;
        }

        public async Task UpdateOrganization(Institution institutionForUpdate)
        {
            if (Enum.IsDefined(typeof(OrganizationType), institutionForUpdate.OrganizationType) && institutionForUpdate.Level < Level.Third)
            {
                var institution = await context.Institutions
                    .SingleOrDefaultAsync(e => e.Id == institutionForUpdate.Id);

                EntityHelper.ClearSkipProperties(institutionForUpdate);

                if (institution != null)
                {
                    EntityHelper.Update(institution, institutionForUpdate, context);
                    await context.SaveChangesAsync();
                }
                else
                {
                    await context.Institutions.AddAsync(institutionForUpdate);
                    await context.SaveChangesAsync();
                }
            }
        }
    }
}

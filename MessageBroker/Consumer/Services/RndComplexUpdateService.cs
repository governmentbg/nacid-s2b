using Microsoft.EntityFrameworkCore;
using Sc.Models;
using Sc.Models.Entities.Nomenclatures.Complexes;
using Sc.Repositories.Helpers;

namespace MessageBroker.Consumer.Services
{
    public class RndComplexUpdateService
    {
        private readonly ScDbContext context;

        public RndComplexUpdateService(
            ScDbContext context
            )
        {
            this.context = context;
        }

        public async Task UpdateComplex(Complex complexForUpdate)
        {
            var complex = await context.Complexes
                .Include(e => e.ComplexOrganizations)
                .SingleOrDefaultAsync(e => e.Id == complexForUpdate.Id);

            EntityHelper.ClearSkipProperties(complexForUpdate);

            if (complex != null)
            {
                EntityHelper.Update(complex, complexForUpdate, context);
                await context.SaveChangesAsync();
            }
            else
            {
                await context.Complexes.AddAsync(complexForUpdate);
                await context.SaveChangesAsync();
            }
        }
    }
}

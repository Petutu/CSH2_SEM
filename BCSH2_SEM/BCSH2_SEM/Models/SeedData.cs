using BCSH2_SEM.Data;
using Microsoft.EntityFrameworkCore;

namespace BCSH2_SEM.Models
{
    public static class SeedData
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new BCSH2_SEMContext(
                serviceProvider.GetRequiredService<
                    DbContextOptions<BCSH2_SEMContext>>()))
            {
                // Look for any movies.
                if (context.Member.Any())
                {
                    return;   // DB has been seeded
                }
                context.Member.AddRange(
                    new Member
                    {
                        
                        FirstName = "Jace",
                        LastName = "Romantic Comedy",
                        MembershipType = "i",
                        JoinDate = DateTime.Parse("1984-3-13")
                    }
                   
                );
                context.SaveChanges();
            }
        }
    }
}

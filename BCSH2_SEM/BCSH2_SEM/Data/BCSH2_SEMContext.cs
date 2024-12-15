using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using BCSH2_SEM.Models;

namespace BCSH2_SEM.Data
{
    public class BCSH2_SEMContext : DbContext
    {
        public BCSH2_SEMContext (DbContextOptions<BCSH2_SEMContext> options)
            : base(options)
        {
        }

        public DbSet<BCSH2_SEM.Models.Member> Member { get; set; } = default!;

        public DbSet<BCSH2_SEM.Models.Session> Session { get; set; } = default!;

        public DbSet<BCSH2_SEM.Models.Trainer> Trainer { get; set; } = default!;
    }
}

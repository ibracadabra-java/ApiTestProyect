using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.CodeAnalysis;

namespace ApiTestProyect.Models
{
    public class MarketContext : DbContext
    {
        public MarketContext(DbContextOptions<MarketContext> options)
            : base(options)
        {
        }

        public DbSet<Market> Markets { get; set; } = null!;

    }
}

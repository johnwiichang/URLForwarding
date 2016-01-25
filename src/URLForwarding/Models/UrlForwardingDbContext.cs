using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Data.Entity;
using URLForwarding.Models;
using Microsoft.Data.Entity.Infrastructure;

namespace URLForwarding.Models
{
    public class UrlForwardingDbContext : DbContext
    {
        public DbSet<UrlRecord> Urls { get; set; }
    }
}

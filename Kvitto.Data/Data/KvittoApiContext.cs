using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Kvitto.Core.Entities;

namespace Kvitto.Data.Data
{
    public class KvittoApiContext : DbContext
    {
        public KvittoApiContext(DbContextOptions<KvittoApiContext> options)
            : base(options)
        {
        }
        public DbSet<Receipt> Receipt { get; set; } = default!;

        public DbSet<UploadedFile> UploadedFile { get; set; } = default!;
    }
}

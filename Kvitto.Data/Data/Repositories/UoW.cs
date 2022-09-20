using Kvitto.Core.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kvitto.Data.Data.Repositories
{
    public class UoW : IDevUoW, IUoW
    {
        public IReceiptRepository ReceiptRepository { get; }

        private readonly KvittoApiContext db;

        public UoW(KvittoApiContext db)
        {
            this.db = db;
            ReceiptRepository = new ReceiptRepository(db);
        }

        public async Task CompleteAsync()
        {
            await db.SaveChangesAsync();
        }

        public void EnsureDeleted()
        {
            db.Database.EnsureDeleted();
        }
        public void Migrate()
        {
            db.Database.Migrate();
        }
        
    }
}

using Kvitto.Core.Entities;
using Kvitto.Core.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kvitto.Data.Data.Repositories
{
    public class ReceiptRepository : IReceiptRepository
    {
        private readonly KvittoApiContext db;
        public ReceiptRepository(KvittoApiContext db)
        {
            this.db = db;
        }
        public void Add(Receipt receipt)
        {
            db.Add(receipt);
        }

        public async Task<bool> AnyAsync(int? id)
        {
            return await db.Receipt.AnyAsync(c => c.Id == id);
        }

        public async Task<Receipt?> FindAsync(int? id)
        {
            return await db.Receipt.FindAsync(id);
        }

        public async Task<IEnumerable<Receipt>> GetAllReceipts()
        {
            return await db.Receipt.ToListAsync();
        }

        public async Task<Receipt?> GetReceipt(int? id)
        {
            return await db.Receipt.FirstAsync(c => c.Id == id);
        }

        public void Remove(Receipt receipt)
        {
            db.Receipt.Remove(receipt);
        }

        public void Update(Receipt receipt)
        {
            db.Entry(receipt).State = EntityState.Modified;
        }
    }
}

using Kvitto.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kvitto.Core.Repositories
{
    public interface IReceiptRepository
    {
        Task<IEnumerable<Receipt>> GetAllReceipts();
        Task<Receipt?> GetReceipt(int? id);
        Task<Receipt?> FindAsync(int? id);
        Task<bool> AnyAsync(int? id);
        void Add(Receipt receipt);
        void Update(Receipt receipt);
        void Remove(Receipt receipt);
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kvitto.Core.Repositories
{
    public interface IUoW
    {
        IReceiptRepository ReceiptRepository { get; }

        Task CompleteAsync();
    }
}

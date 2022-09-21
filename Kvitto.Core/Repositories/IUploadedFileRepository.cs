using Kvitto.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kvitto.Core.Repositories
{
    public interface IUploadedFileRepository
    {
        void Add(UploadedFile file);
        void ConnectUploadedFileToReceipt(UploadedFile file, int receiptId);
    }
}

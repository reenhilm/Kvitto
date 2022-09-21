using Kvitto.Core.Entities;
using Kvitto.Core.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kvitto.Data.Data.Repositories
{
    public class UploadedFileRepository : IUploadedFileRepository
    {
        private readonly KvittoApiContext db;
        public UploadedFileRepository(KvittoApiContext db)
        {
            this.db = db;
        }
        public void Add(UploadedFile file)
        {
            db.Add(file);
            
        }
        public void ConnectUploadedFileToReceipt(UploadedFile file, int receiptId)
        {
            file.ReceiptId = receiptId;
            Update(file);
        }

        protected void Update(UploadedFile file)
        {
            db.Entry(file).State = EntityState.Modified;
        }
    }
}

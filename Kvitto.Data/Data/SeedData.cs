using Kvitto.Core.Repositories;
using Kvitto.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kvitto.Data.Data
{
    public static class SeedData
    {
        public static async Task InitAsync(IUoW uow)
        {
            if (uow is null) throw new ArgumentNullException(nameof(uow));

            uow.ReceiptRepository.Add(
                new Receipt()
                {
                    
                    PurchaseDate = DateTime.Now,
                    Description = "ReceiptDesc",
                    ReceiptType = new ReceiptType()
                    {
                        Name = "Elektronik"
                    },
                    StoreName = "Komplett",
                    UploadedDate = DateTime.Now,
                    UploadedFiles = new List<UploadedFile>()
                    {
                        new UploadedFile()
                        {
                            ContentType = "image/x-png"
                        }
                    }
                });

            uow.UploadedFileRepository.Add(
                new UploadedFile()
                {
                    ContentType = "image/x-png"                    
                });

            uow.UploadedFileRepository.Add(
                new UploadedFile()
                {
                    ContentType = "image/x-png"
                });

            await uow.CompleteAsync();
        }
    }
}

using AutoMapper;
using Kvitto.Common.Dto;
using Kvitto.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Kvitto.Data.Data
{
    public class KvittoMappings : Profile
    {
        public KvittoMappings()
        {
            CreateMap<Receipt, ReceiptDto>().ReverseMap();
        }
    }
}

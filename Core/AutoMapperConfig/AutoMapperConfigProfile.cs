using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicketsCRUD.Core.DTO;
using TicketsCRUD.Core.Entites;

namespace TicketsCRUD.Core.AutoMapperConfig
{
    public class AutoMapperConfigProfile : Profile
    {
        public AutoMapperConfigProfile() 
        {
            //Tickets
            CreateMap<CreateTicketDto, Ticket>();
            CreateMap<Ticket , GetTicketDto>();
            CreateMap<UpdateTicketDto , Ticket>();
        }
    }
}

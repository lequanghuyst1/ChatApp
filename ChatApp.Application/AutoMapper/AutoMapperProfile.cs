using AutoMapper;
using ChatApp.Application.DTOs;
using ChatApp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatApp.Application.AutoMapper
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<MessageDTO, Message>().ReverseMap();
            CreateMap<ChatDTO, Chat>().ReverseMap();
            CreateMap<ReactionDTO, Reaction>().ReverseMap();
            CreateMap<ChatParticipantDTO, ChatParticipant>().ReverseMap();
            CreateMap<FriendDTO, FriendDTO>().ReverseMap();
        }
    }
}

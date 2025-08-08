using System;
using ChatApp.Domain.Enums;

namespace ChatApp.Application.DTOs
{
    public class FriendDTO
    {
        public long ID { get; set; }
        public long UserID { get; set; }
        public long FriendID { get; set; }
        public FriendStatus Status { get; set; }
        public DateTime AddedAt { get; set; }
    }
}
using ChatApp.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatApp.Application.DTOs
{
    public class ChatParticipantDTO
    {
        public long ID { get; set; }
        public long ChatID { get; set; }
        public long UserID { get; set; }
        public string FullName { get; set; }
        public string Avatar { get; set; }
        public ChatParticipantType Role { get; set; }
        public DateTime JoinedAt { get; set; }
    }
}

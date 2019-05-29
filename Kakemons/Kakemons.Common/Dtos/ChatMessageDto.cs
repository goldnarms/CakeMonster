using System;
using Kakemons.Common.Enums;

namespace Kakemons.Common.Dtos
{
    public class ChatMessageDto
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public string BakerId { get; set; }
        public bool IsDeleted { get; set; }
        public string Message { get; set; }
        public DateTimeOffset DateSent { get; set; }
        public ChatDirection Direction { get; set; }
    }
}

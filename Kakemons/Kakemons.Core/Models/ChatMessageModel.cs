using System;
using Kakemons.Common.Enums;

namespace Kakemons.Core.Models
{
    public class ChatMessageModel
    {
        public int Id { get; set; }
        public string Message { get; set; }
        public DateTimeOffset DateSent { get; set; }
        public ChatDirection Direction { get; set; }
        public string BakerId { get; set; }
        public string UserId { get; set; }
        public string AvatarUrl { get; set; }
    }
}

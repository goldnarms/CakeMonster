using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kakemons.Common.Dtos;
using Kakemons.Common.Enums;
using Kakemons.SDK.ApiContracts;

namespace Kakemons.SDK.ApiServices
{
    public class ChatApiService : IChatApiService
    {
        private List<ChatMessageDto> _testChatMessages = new List<ChatMessageDto>
        {
            new ChatMessageDto
            {
                Id = 1,
                UserId = "1",
                BakerId = "1",
                IsDeleted = false,
                Message = "Hei",
                DateSent = new DateTimeOffset(2019, 1, 1, 10, 10, 10, TimeSpan.Zero),
                Direction = ChatDirection.UserToBaker
            },
            new ChatMessageDto
            {
                Id = 2,
                UserId = "1",
                BakerId = "1",
                IsDeleted = false,
                Message = "Hei, hva kan jeg hjelpe deg med",
                DateSent = new DateTimeOffset(2019, 1, 1, 10, 15, 10, TimeSpan.Zero),
                Direction = ChatDirection.BakerToUser
            }
        };

        public Task<IEnumerable<ChatMessageDto>> GetChatsForUser(string userId)
        {
            return Task.FromResult(_testChatMessages.Where(cm =>
                !cm.IsDeleted && cm.UserId == userId));
        }

        public Task SendMessage(ChatMessageDto chatMessage)
        {
            _testChatMessages.Add(chatMessage);
            return Task.CompletedTask;
        }

        public Task DeleteMessage(int id)
        {
            var chatMessage = _testChatMessages.FirstOrDefault(cm => cm.Id == id);
            if(chatMessage != null)
                _testChatMessages.RemoveAt(_testChatMessages.IndexOf(chatMessage));
            return Task.CompletedTask;
        }
    }
}

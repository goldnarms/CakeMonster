using System.Collections.Generic;
using System.Threading.Tasks;
using Kakemons.Common.Dtos;

namespace Kakemons.SDK.ApiContracts
{
    public interface IChatApiService
    {
        Task DeleteMessage(int id);
        Task<IEnumerable<ChatMessageDto>> GetChatsForUser(string userId);
        Task SendMessage(ChatMessageDto chatMessage);
    }
}

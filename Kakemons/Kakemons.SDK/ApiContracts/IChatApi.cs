using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Kakemons.Common.Dtos;
using Refit;

namespace Kakemons.SDK.ApiContracts
{
    public interface IChatApi
    {
        [Delete("/Chat")]
        Task DeleteMessage(int id);

        [Get("/User/{id}/Chats")]
        Task<IEnumerable<ChatMessageDto>> GetChatsForUser(string id);

        [Post("/Chat")]
        Task SendMessage([Body]ChatMessageDto chatMessage);
    }
}

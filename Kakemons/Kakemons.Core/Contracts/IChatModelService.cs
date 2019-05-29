using System.Threading.Tasks;
using DynamicData;
using Kakemons.Common.Dtos;

namespace Kakemons.Core.Contracts
{
    public interface IChatModelService
    {
        IObservableCache<ChatMessageDto, int> ChatsForUser { get; }

        Task DeleteChatMessage(int id);
        Task SendChatMessage(ChatMessageDto chatMessageDto);
    }
}

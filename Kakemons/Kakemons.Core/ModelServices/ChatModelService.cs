using System;
using System.Linq;
using System.Reactive.Linq;
using System.Threading.Tasks;
using DynamicData;
using Kakemons.Common.Dtos;
using Kakemons.Core.Contracts;
using Kakemons.Core.Extensions;
using Kakemons.SDK.ApiContracts;
using ReactiveUI;
using Serilog;

namespace Kakemons.Core.ModelServices
{
    public class ChatModelService : IChatModelService
    {
        private readonly IChatApiService _chatApiService;
        private readonly ILogger _logger;
        private readonly SourceCache<ChatMessageDto, int> _chatsForUser;
        private readonly SourceCache<BakerDto, string> _bakerChatsForUser;

        public ChatModelService(IAppUserModelService appUserModelService, IChatApiService chatApiService, IBakerModelService bakerModelService, ILogger logger)
        {
            _chatApiService = chatApiService;
            _logger = logger;
            _chatsForUser = new SourceCache<ChatMessageDto, int>(cm => cm.Id);
            _bakerChatsForUser = new SourceCache<BakerDto, string>(bk => bk.Id);

            appUserModelService
                .UserObservable
                .ObserveOn(RxApp.TaskpoolScheduler)
                .Where(u => u != null)
                .SelectMany(async u => await chatApiService.GetChatsForUser(u.Id))
                .RetryWithBackoff(3)
                .LogException(logger)
                .Subscribe(async chatMessages =>
                {
                    _chatsForUser.Edit(l => l.AddOrUpdate(chatMessages));
                    var bakerIds = chatMessages.GroupBy(cm => cm.BakerId).Select(b => b.Key);

                    foreach (var bakerId in bakerIds)
                    {
                        var baker = await bakerModelService.GetBaker(bakerId);
                        _bakerChatsForUser.Edit(l => l.AddOrUpdate(baker)); //TODO: optimize
                    }
                });
        }

        public IObservableCache<ChatMessageDto, int> ChatsForUser => _chatsForUser;

        public async Task DeleteChatMessage(int id)
        {
            try
            {
                await _chatApiService.DeleteMessage(id);
                _chatsForUser.Edit(l => l.RemoveKey(id));
            }
            catch (Exception ex)
            {
                _logger.Error(nameof(DeleteChatMessage), ex);
                throw;
            }
        }

        public async Task SendChatMessage(ChatMessageDto chatMessageDto)
        {
            try
            {
                await _chatApiService.SendMessage(chatMessageDto);
                _chatsForUser.Edit(l => l.AddOrUpdate(chatMessageDto));
            }
            catch (Exception ex)
            {
                _logger.Error(nameof(SendChatMessage), ex);
                throw;
            }
        }
    }
}

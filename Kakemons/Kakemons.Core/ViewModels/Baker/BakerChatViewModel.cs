using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reactive;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Threading.Tasks;
using DynamicData;
using DynamicData.Binding;
using Kakemons.Common.Dtos;
using Kakemons.Common.Enums;
using Kakemons.Core.Contracts;
using Kakemons.Core.Models;
using ReactiveUI;
using Splat;

namespace Kakemons.Core.ViewModels.Baker
{
    public class BakerChatViewModel : BaseViewModel
    {
        private readonly IAppUserModelService _appUserModelService;
        private readonly IBakerModelService _bakerModelService;
        private readonly IChatModelService _chatModelService;
        private string _bakerId = "1";
        private readonly ReadOnlyObservableCollection<ChatMessageModel> _chatMessages;
        private string _message;

        public BakerChatViewModel(string bakerId,
            IAppUserModelService appUserModelService = null, 
            IBakerModelService bakerModelService = null, 
            IChatModelService chatModelService = null)
        {
            _appUserModelService = appUserModelService ?? Locator.Current.GetService<IAppUserModelService>();
            _bakerModelService = bakerModelService;
            _chatModelService = chatModelService;

            var user = _appUserModelService.User;
            UserAvatarUrl = user.AvatarUrl;

            var dynamicFilter = this.WhenValueChanged(vm => vm.BakerId)
                .Select(CreatePredicate);

            var chatMessages = _chatModelService
                .ChatsForUser
                .Connect()
                .SubscribeOn(RxApp.TaskpoolScheduler)
                .Filter(dynamicFilter)
                .Transform(TransformToListItem)
                .Publish();

            chatMessages
                .Bind(out _chatMessages)
                .ObserveOn(RxApp.MainThreadScheduler)
                .Subscribe()
                .DisposeWith(CompositeDisposable);

            chatMessages.Connect();

            SendMessageCommand = ReactiveCommand.CreateFromTask(SendMessage);

            PrepareCommand = ReactiveCommand.CreateFromTask(async b => await Prepare(bakerId));

            Observable.Return(Unit.Default)
                .InvokeCommand(PrepareCommand);

            PrepareCommand.Subscribe();
        }

        public ReactiveCommand<Unit, Unit> PrepareCommand { get; set; }

        private async Task SendMessage()
        {
            if(string.IsNullOrEmpty(Message))
                return;

            await _chatModelService.SendChatMessage(new ChatMessageDto
            {
                BakerId = BakerId, DateSent = DateTimeOffset.Now, Direction = ChatDirection.UserToBaker,
                Message = Message, UserId = _appUserModelService.UserId
            });

            Message = string.Empty;
        }

        public ReactiveCommand<Unit, Unit> SendMessageCommand { get; set; }
        public string UserAvatarUrl { get; set; }

        public string Message
        {
            get => _message;
            set => this.RaiseAndSetIfChanged(ref _message, value);
        }

        public async Task Prepare(string bakerId)
        {
            var baker = await _bakerModelService.GetBaker(bakerId);

            if (baker != null)
            {
                BakerId = bakerId;
                BakerAvatarUrl = baker.AvatarUrl;
            }
        }

        public string BakerAvatarUrl { get; set; }

        public string BakerId
        {
            get => _bakerId;
            set => this.RaiseAndSetIfChanged(ref _bakerId, value);
        }

        public IReadOnlyCollection<ChatMessageModel> ChatMessages => _chatMessages;

        private Func<ChatMessageDto, bool> CreatePredicate(string bakerId)
        {
            return chatMessage => chatMessage.UserId == _appUserModelService.UserId && chatMessage.BakerId == bakerId;
        }

        private ChatMessageModel TransformToListItem(ChatMessageDto dto) => new ChatMessageModel
        {
            Id = dto.Id,
            Message = dto.Message,
            DateSent = dto.DateSent,
            Direction = dto.Direction,
            BakerId = dto.BakerId,
            UserId = dto.UserId,
            AvatarUrl = dto.Direction == ChatDirection.BakerToUser ? BakerAvatarUrl : UserAvatarUrl
        };
    }
}

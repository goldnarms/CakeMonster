using System;
using Kakemons.Common.Enums;
using Kakemons.Core.Models;
using Kakemons.UI.Controls.ListViews.ViewCells;
using Xamarin.Forms;

namespace Kakemons.UI.TemplateSelectors
{
    public class ChatListTemplateSelector : DataTemplateSelector
    {
        private readonly DataTemplate _chatUserToBakerTemplate;
        private readonly DataTemplate _chatBakerToUserTemplate;

        public ChatListTemplateSelector()
        {
            _chatUserToBakerTemplate = new DataTemplate(typeof(ChatCellUserToBaker));
            _chatBakerToUserTemplate = new DataTemplate(typeof(ChatCellBakerToUser));
        }
        protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
        {
            if (!(item is ChatMessageModel chatMessage))
                throw new ArgumentOutOfRangeException($"Model type is not valid. Needs to be {nameof(ChatMessageModel)}");

            return chatMessage.Direction == ChatDirection.BakerToUser ? _chatBakerToUserTemplate : _chatUserToBakerTemplate;
            
        }
    }
}

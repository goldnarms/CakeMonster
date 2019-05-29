using System;
using System.Collections.Generic;
using System.Text;
using ReactiveUI;

namespace Kakemons.Core.ViewModels.Onboarding
{
    public class OnboardingCardItem: ReactiveObject
    {
        private string _image;
        private string _header;
        private string _text;
        private bool _isLastPage;

        public string Image
        {
            get => _image;
            set => this.RaiseAndSetIfChanged(ref _image, value);
        }

        public string Header
        {
            get => _header;
            set => this.RaiseAndSetIfChanged(ref _header, value);
        }

        public string Text
        {
            get => _text;
            set => this.RaiseAndSetIfChanged(ref _text, value);
        }

        public bool IsLastPage
        {
            get => _isLastPage;
            set => this.RaiseAndSetIfChanged(ref _isLastPage, value);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using Kakemons.Core.Contracts;

namespace Kakemons.Core.Services
{
    public class DialogService : IDialogService
    {
        public Task AlertAsync(string header, string body)
        {
            Debug.WriteLine(header + ", " + body);
            return Task.CompletedTask;
        }

        public Task ActionSheetAsync(Dictionary<string, string> actionConfig)
        {
            Debug.WriteLine(actionConfig.Values.ToString());
            return Task.CompletedTask;
        }
        public Task PresentAlertAsync(string title, string body)
        {
            Debug.WriteLine(title + ", " + body);
            return Task.CompletedTask;
        }
    }
}

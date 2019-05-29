using System.Collections.Generic;
using System.Threading.Tasks;

namespace Kakemons.Core.Contracts
{
    public interface IDialogService
    {
        Task AlertAsync(string header, string body);
        Task ActionSheetAsync(Dictionary<string, string> actionConfig);
        Task PresentAlertAsync(string title, string body);
    }
}

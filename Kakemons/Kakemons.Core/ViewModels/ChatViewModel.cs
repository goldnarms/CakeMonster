using Serilog;
using System;
using System.Collections.Generic;
using System.Text;

namespace Kakemons.Core.ViewModels
{
    public class ChatViewModel: BaseViewModel
    {
        public ChatViewModel(int id, ILogger log)
        {
            Prepare(id);
        }

        public void Prepare(int parameter)
        {
            
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace Kakemons.Common.Contracts
{
    public interface ILogger
    {
        void Debug(string s);
        void LogError(string message, Exception exception);
        void LogError(string message);
        void Trace(string message);
    }
}

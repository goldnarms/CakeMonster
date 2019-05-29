using System;
using System.Reactive.Linq;
using ReactiveUI;

namespace Kakemons.Core.Models
{
    public class MvxReactiveObject : ReactiveObject, IDisposable
    {
        public MvxReactiveObject()
        {
        }

        public void Dispose()
        {
        }
    }
}

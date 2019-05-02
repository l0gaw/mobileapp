using System;
using System.Collections.Generic;
using System.Reactive;
using Toggl.Core.UI.Services;

namespace Toggl.Droid.Fragments
{
    public abstract partial class ReactiveFragment<TViewModel>
    {
        public IObservable<bool> Confirm(string title, string message, string confirmButtonText, string dismissButtonText)
        {
            throw new NotImplementedException();
        }

        public IObservable<Unit> Alert(string title, string message, string buttonTitle)
        {
            throw new NotImplementedException();
        }

        public IObservable<bool> ConfirmDestructiveAction(ActionType type, params object[] formatArguments)
        {
            throw new NotImplementedException();
        }

        public IObservable<T> Select<T>(string title, IEnumerable<(string ItemName, T Item)> options, int initialSelectionIndex)
        {
            throw new NotImplementedException();
        }
    }
}
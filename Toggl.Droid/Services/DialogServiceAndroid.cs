using System;
using System.Collections.Generic;
using System.Reactive;
using Toggl.Core.UI.Services;
using Object = Java.Lang.Object;

namespace Toggl.Droid.Services
{
    public sealed class DialogServiceAndroid : Object, IDialogService
    {
        public IObservable<bool> Confirm(string title, string message, string confirmButtonText, string dismissButtonText)
        {
            throw new NotImplementedException("Use The IView IDialogService implementation");
        }

        public IObservable<Unit> Alert(string title, string message, string buttonTitle)
        {
            throw new NotImplementedException("Use The IView IDialogService implementation");
        }

        public IObservable<bool> ConfirmDestructiveAction(ActionType type, params object[] formatArguments)
        {
            throw new NotImplementedException("Use The IView IDialogService implementation");
        }

        public IObservable<T> Select<T>(string title, IEnumerable<(string ItemName, T Item)> options, int initialSelectionIndex)
        {
            throw new NotImplementedException("Use The IView IDialogService implementation");
        }
    }
}

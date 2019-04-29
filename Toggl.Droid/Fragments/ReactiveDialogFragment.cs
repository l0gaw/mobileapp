using System;
using System.Reactive.Disposables;
using Android.Runtime;
using Android.Support.V4.App;
using Android.Views;
using Toggl.Core.UI.ViewModels;

namespace Toggl.Droid.Fragments
{
    public abstract class ReactiveDialogFragment<TViewModel> : DialogFragment
        where TViewModel : IViewModelLifecycle
    {
        protected CompositeDisposable DisposeBag = new CompositeDisposable();

        public TViewModel ViewModel { get; set; }

        public ReactiveDialogFragment()
            : base() { }

        public ReactiveDialogFragment(IntPtr javaReference, JniHandleOwnership transfer)
            : base(javaReference, transfer) { }

        protected abstract void InitializeViews(View view);

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);

            if (!disposing) return;
            DisposeBag?.Dispose();
        }
    }
}

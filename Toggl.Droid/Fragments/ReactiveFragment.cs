using System;
using System.Reactive.Disposables;
using System.Threading.Tasks;
using Android.Runtime;
using Android.Support.V4.App;
using Android.Views;
using Toggl.Core.UI.ViewModels;
using Toggl.Core.UI.Views;

namespace Toggl.Droid.Fragments
{
    public abstract partial class ReactiveFragment<TViewModel> : Fragment, IView
        where TViewModel : class, IViewModel
    {
        protected CompositeDisposable DisposeBag = new CompositeDisposable();

        public TViewModel ViewModel { get; set; }

        protected abstract void InitializeViews(View view);

        protected ReactiveFragment()
        {
        }

        protected ReactiveFragment(IntPtr javaReference, JniHandleOwnership transfer)
            : base(javaReference, transfer)
        {
        }

        public override void OnDestroyView()
        {
            base.OnDestroyView();
            DisposeBag.Dispose();
            DisposeBag = new CompositeDisposable();
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);

            if (!disposing) return;
            DisposeBag?.Dispose();
        }

        public Task Close()
        {
            return Task.CompletedTask;
        }
    }
}

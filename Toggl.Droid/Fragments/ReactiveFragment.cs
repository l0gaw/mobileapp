using System.Reactive.Disposables;
using Android.Support.V4.App;
using Android.Views;
using Toggl.Core.UI.ViewModels;

namespace Toggl.Droid.Fragments
{
    public abstract class ReactiveFragment<TViewModel> : Fragment
        where TViewModel : IViewModelLifecycle
    {
        protected CompositeDisposable DisposeBag = new CompositeDisposable();

        public TViewModel ViewModel { get; set; }

        protected abstract void InitializeViews(View view);

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
    }
}

using System.Reactive.Disposables;
using Android.OS;
using Android.Views;
using MvvmCross.Droid.Support.V4;
using MvvmCross.ViewModels;
using Toggl.Core.UI.ViewModels;
using Toggl.Core.UI.Views;

namespace Toggl.Droid.Fragments
{
    public abstract partial class ReactiveFragment<TViewModel> : MvxFragment<TViewModel>, IView
        where TViewModel : class, IMvxViewModel, IViewModel
    {
        protected CompositeDisposable DisposeBag = new CompositeDisposable();

        protected abstract void InitializeViews(View view);

        public override void OnViewCreated(View view, Bundle savedInstanceState)
        {
            base.OnViewCreated(view, savedInstanceState);
            ViewModel.AttachView(this);
        }

        public override void OnDestroyView()
        {
            base.OnDestroyView();
            ViewModel.DetachView();
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

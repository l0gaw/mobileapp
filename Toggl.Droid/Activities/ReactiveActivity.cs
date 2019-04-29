using System;
using System.Reactive.Disposables;
using System.Threading.Tasks;
using Android.Content;
using Android.Content.PM;
using Android.Runtime;
using Android.Support.V7.App;
using Toggl.Core.UI.ViewModels;
using static Toggl.Droid.Services.PermissionsServiceAndroid;

namespace Toggl.Droid.Activities
{
    public abstract class ReactiveActivity<TViewModel> : AppCompatActivity, IView, IPermissionAskingActivity
        where TViewModel : IViewModelLifecycle
    {
        public CompositeDisposable DisposeBag { get; private set; } = new CompositeDisposable();

        protected abstract void InitializeViews();
        
        public TViewModel ViewModel { get; }
        
        public Action<int, string[], Permission[]> OnPermissionChangedCallback { get; set; }

        protected ReactiveActivity()
        {
            ViewModel = AndroidDependencyContainer.Instance
                .ActivityPresenter
                .GetCachedViewModel<TViewModel>();
        }

        protected ReactiveActivity(IntPtr javaReference, JniHandleOwnership transfer)
            : base(javaReference, transfer)
        {
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            ViewModel?.ViewDestroyed();
        }

        protected override void OnStart()
        {
            base.OnResume();
            ViewModel?.ViewAttached(this);
        }

        protected override void OnResume()
        {
            base.OnResume();
            ViewModel?.ViewAppeared();
        }

        protected override void OnPause()
        {
            base.OnPause();
            ViewModel?.ViewDisappeared();
        }

        protected override void OnStop()
        {
            base.OnStop();
            ViewModel?.ViewDetached();
        }

        public void MvxInternalStartActivityForResult(Intent intent, int requestCode)
        {
            StartActivityForResult(intent, requestCode);
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);

            if (!disposing) return;
            DisposeBag?.Dispose();
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, Permission[] grantResults)
        {
            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            OnPermissionChangedCallback?.Invoke(requestCode, permissions, grantResults);
            OnPermissionChangedCallback = null;
        }

        public Task Close()
        {
            Finish();
            return Task.CompletedTask;
        }
    }
}

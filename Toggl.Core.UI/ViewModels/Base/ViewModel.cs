using System.Reactive;
using System.Threading.Tasks;

namespace Toggl.Core.UI.ViewModels
{
    public interface IView
    {
        Task Close();
    }

    public interface IViewModelLifecycle
    {
        void ViewAttached(IView view);

        void ViewDetached();

        void ViewAppeared();

        void ViewDisappeared();

        void ViewDestroyed();
    }

    public abstract class ViewModel<TInput, TOutput> : IViewModelLifecycle
    {
        public IView View { get; private set; }

        internal TaskCompletionSource<TOutput> CloseCompletionSource { get; set; }

        public virtual void Initialize(TInput input)
        {
        }

        public async Task CloseView(TOutput output)
        {
            await View.Close();
            CloseCompletionSource.SetResult(output);
        }

        public void ViewAttached(IView view)
        {
            View = view;
        }

        public void ViewDetached()
        {
            View = null;
        }

        public virtual void ViewAppeared()
        {
        }

        public virtual void ViewDisappeared()
        {
        }

        public virtual void ViewDestroyed()
        {
        }
    }

    public abstract class ViewModel : ViewModel<Unit, Unit>
    {
        public Task CloseView() => CloseView(Unit.Default);

        public virtual void Initialize()
        {
        }
        
        public override sealed void Initialize(Unit input)
        {
            Initialize();
        }
    }
}

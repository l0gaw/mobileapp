using System.Reactive;

namespace Toggl.Core.UI.ViewModels
{
    public abstract class ViewModelWithOutput<TOutput> : ViewModel<Unit, TOutput>
    {
        public virtual void Initialize()
        {
        }

        public override sealed void Initialize(Unit input)
        {
            Initialize();
        }
    }
}

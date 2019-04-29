using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using Toggl.Core.UI.Navigation;
using Toggl.Core.UI.ViewModels;
using Android.App;

namespace Toggl.Droid.Presentation
{
    public abstract class AndroidPresenter : IPresenter
    {
        protected abstract HashSet<Type> AcceptedViewModels { get; }

        protected abstract void PresentOnMainThread<TInput, TOutput>(ViewModel<TInput, TOutput> viewModel);

        public Task Present<TInput, TOutput>(ViewModel<TInput, TOutput> viewModel)
        {
            var tcs = new TaskCompletionSource<object>();
            Application.SynchronizationContext.Post(_ =>
            {
                PresentOnMainThread(viewModel);
                tcs.SetResult(true);
            }, null);

            return tcs.Task;
        }
        
        public virtual bool CanPresent<TInput, TOutput>(ViewModel<TInput, TOutput> viewModel)
            => AcceptedViewModels.Contains(viewModel.GetType());
    }
}

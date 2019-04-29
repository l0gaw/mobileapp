using System.Threading.Tasks;
using Toggl.Core.Analytics;
using Toggl.Core.UI.ViewModels;
using Toggl.Shared;

namespace Toggl.Core.UI.Navigation
{
    public sealed class NavigationService : INavigationService
    {
        private readonly CompositePresenter presenter;
        private readonly TogglViewModelLocator locator;
        private readonly IAnalyticsService analyticsService;

        public NavigationService(CompositePresenter presenter, TogglViewModelLocator locator, IAnalyticsService analyticsService)
        {
            Ensure.Argument.IsNotNull(presenter, nameof(presenter));
            Ensure.Argument.IsNotNull(analyticsService, nameof(analyticsService));

            this.locator = locator;
            this.presenter = presenter;
            this.analyticsService = analyticsService;
        }

        public async Task<TOutput> Navigate<TViewModel, TInput, TOutput>(TInput payload)
            where TViewModel : ViewModel<TInput, TOutput>
        {
            var viewModel = locator.Load<TInput, TOutput>(typeof(TViewModel), payload);
            await presenter.Present(viewModel);

            analyticsService.CurrentPage.Track(typeof(TViewModel));
            
            viewModel.CloseCompletionSource = new TaskCompletionSource<TOutput>();
            return await viewModel.CloseCompletionSource.Task;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using Android.App;
using Android.Content;
using Toggl.Core.UI.ViewModels;
using Toggl.Core.UI.ViewModels.Settings;
using Toggl.Droid.Activities;

namespace Toggl.Droid.Presentation
{
    public sealed class ActivityPresenter : AndroidPresenter
    {
        private readonly Dictionary<Type, IViewModelLifecycle> temporaryViewModelCache = new Dictionary<Type, IViewModelLifecycle>();
        private readonly Dictionary<Type, Type> viewModelToActivityMap = new Dictionary<Type, Type>
        {
            { typeof(AboutViewModel), typeof(AboutActivity) },
            { typeof(BrowserViewModel), typeof(BrowserActivity) },
            { typeof(CalendarSettingsViewModel), typeof(CalendarSettingsActivity) },
            { typeof(EditDurationViewModel), typeof(EditDurationActivity) },
            { typeof(EditProjectViewModel), typeof(EditProjectActivity) },
            { typeof(EditTimeEntryViewModel), typeof(EditTimeEntryActivity) },
            { typeof(ForgotPasswordViewModel), typeof(ForgotPasswordActivity) },
            { typeof(LoginViewModel), typeof(LoginActivity) },
            { typeof(MainTabBarViewModel), typeof(MainTabBarActivity) },
            { typeof(OutdatedAppViewModel), typeof(OutdatedAppActivity) },
            { typeof(SelectClientViewModel), typeof(SelectClientActivity) },
            { typeof(SelectCountryViewModel), typeof(SelectCountryActivity) },
            { typeof(SelectProjectViewModel), typeof(SelectProjectActivity) },
            { typeof(SelectTagsViewModel), typeof(SelectTagsActivity) },
            { typeof(SendFeedbackViewModel), typeof(SendFeedbackActivity) },
            { typeof(SignupViewModel), typeof(SignUpActivity) },
            { typeof(StartTimeEntryViewModel), typeof(StartTimeEntryActivity) },
            { typeof(TokenResetViewModel), typeof(TokenResetActivity) }
        };

        protected override HashSet<Type> AcceptedViewModels { get; }
        
        public ActivityPresenter()
        {
            AcceptedViewModels = viewModelToActivityMap.Keys.ToHashSet();
        }

        protected override void PresentOnMainThread<TInput, TOutput>(ViewModel<TInput, TOutput> viewModel)
        {
            var viewModelType = viewModel.GetType();
            var activityType = viewModelToActivityMap[viewModelType];
            var intent = new Intent(Application.Context, activityType);
            Application.Context.StartActivity(activityType);

            temporaryViewModelCache[viewModelType] = viewModel;
        }

        internal TViewModel GetCachedViewModel<TViewModel>()
            where TViewModel : IViewModelLifecycle
        {
            var viewModelType = typeof(TViewModel);
            var viewModel = (TViewModel)temporaryViewModelCache[viewModelType];
            temporaryViewModelCache.Remove(viewModelType);
            return viewModel;
        }
    }
}

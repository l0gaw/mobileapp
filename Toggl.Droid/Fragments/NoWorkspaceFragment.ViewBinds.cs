using Android.Views;
using Android.Widget;

namespace Toggl.Droid.Fragments
{
    public partial class NoWorkspaceFragment
    {
        private ProgressBar progressBar;
        private TextView tryAgainTextView;
        private TextView createWorkspaceTextView;

        protected override void InitializeViews(View rootView)
        {
            progressBar = rootView.FindViewById<ProgressBar>(Resource.Id.ProgressBar);
            tryAgainTextView = rootView.FindViewById<TextView>(Resource.Id.TryAgainTextView);
            createWorkspaceTextView = rootView.FindViewById<TextView>(Resource.Id.CreateWorkspaceTextView);
        }
    }
}

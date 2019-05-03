﻿using Android.Support.V7.Widget;
using Android.Views;

namespace Toggl.Droid.Fragments
{
    public partial class SelectDefaultWorkspaceFragment
    {
        private RecyclerView recyclerView;

        protected override void InitializeViews(View rootView)
        {
            recyclerView = rootView.FindViewById<RecyclerView>(Resource.Id.SelectWorkspaceRecyclerView);
        }
    }
}

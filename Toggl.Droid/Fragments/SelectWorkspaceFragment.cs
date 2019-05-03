﻿using Android.OS;
using Android.Support.V7.Widget;
using Android.Views;
using MvvmCross.Platforms.Android.Presenters.Attributes;
using Toggl.Core.UI.ViewModels;
using Toggl.Droid.Adapters;
using Toggl.Droid.ViewHolders;
using Toggl.Shared.Extensions;

namespace Toggl.Droid.Fragments
{
    [MvxFragmentPresentation(typeof(EditProjectViewModel), Resource.Id.SelectWorkspaceContainer, AddToBackStack = true)]
    public partial class SelectWorkspaceFragment : ReactiveFragment<SelectWorkspaceViewModel>
    {
        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            base.OnCreateView(inflater, container, savedInstanceState);
            var view = inflater.Inflate(Resource.Layout.SelectWorkspaceFragment, container, false);
            InitializeViews(view);

            var adapter = new SimpleAdapter<SelectableWorkspaceViewModel>(
                Resource.Layout.SelectWorkspaceFragmentCell,
                SelectWorkspaceViewHolder.Create
            );

            adapter.ItemTapObservable
                .Subscribe(ViewModel.SelectWorkspace.Inputs)
                .DisposedBy(DisposeBag);

            adapter.Items = ViewModel.Workspaces;

            recyclerView.SetAdapter(adapter);
            recyclerView.SetLayoutManager(new LinearLayoutManager(Context));

            return view;
        }

        public override void OnDestroy()
        {
            base.OnDestroy();
            DisposeBag.Dispose();
        }
    }
}

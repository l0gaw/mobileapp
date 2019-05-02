﻿using System;
using System.Linq;
using System.Reactive.Linq;
using Android.OS;
using Android.Runtime;
using Android.Support.V7.Widget;
using Android.Views;
using MvvmCross.Platforms.Android.Presenters.Attributes;
using Toggl.Core.UI.ViewModels;
using Toggl.Droid.Adapters;
using Toggl.Droid.ViewHolders;
using Toggl.Shared.Extensions;

namespace Toggl.Droid.Fragments
{
    [MvxDialogFragmentPresentation(AddToBackStack = true, Cancelable = false)]
    public sealed partial class SelectDefaultWorkspaceFragment : ReactiveDialogFragment<SelectDefaultWorkspaceViewModel>
    {
        public SelectDefaultWorkspaceFragment() { }

        public SelectDefaultWorkspaceFragment(IntPtr javaReference, JniHandleOwnership transfer)
            : base(javaReference, transfer) { }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            base.OnCreateView(inflater, container, savedInstanceState);
            var view = inflater.Inflate(Resource.Layout.SelectDefaultWorkspaceFragment, null);
            InitializeViews(view);

            var adapter = new SimpleAdapter<SelectableWorkspaceViewModel>(
                Resource.Layout.SelectDefaultWorkspaceFragmentCell,
                SelectDefaultWorkspaceViewHolder.Create
            );
            adapter.Items = ViewModel.Workspaces.ToList();
            adapter.ItemTapObservable
                .Subscribe(ViewModel.SelectWorkspace.Inputs)
                .DisposedBy(DisposeBag);

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

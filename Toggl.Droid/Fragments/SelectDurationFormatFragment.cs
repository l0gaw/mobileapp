﻿using System;
using System.Linq;
using System.Reactive.Disposables;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Support.V7.Widget;
using Android.Views;
using MvvmCross.Droid.Support.V4;
using MvvmCross.Platforms.Android.Presenters.Attributes;
using Toggl.Core.UI.ViewModels;
using Toggl.Droid.Adapters;
using Toggl.Droid.Extensions;
using Toggl.Shared.Extensions;

namespace Toggl.Droid.Fragments
{
    [MvxDialogFragmentPresentation(AddToBackStack = true)]
    public sealed partial class SelectDurationFormatFragment : ReactiveDialogFragment<SelectDurationFormatViewModel>
    {
        public SelectDurationFormatFragment() { }

        public SelectDurationFormatFragment(IntPtr javaReference, JniHandleOwnership transfer)
            : base (javaReference, transfer) { }

        public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
        {
            base.OnCreateView(inflater, container, savedInstanceState);
            var view = inflater.Inflate(Resource.Layout.SelectDurationFormatFragment, null);

            InitializeViews(view);

            recyclerView.SetLayoutManager(new LinearLayoutManager(Context));
            selectDurationRecyclerAdapter = new SelectDurationFormatRecyclerAdapter();
            selectDurationRecyclerAdapter.Items = ViewModel.DurationFormats.ToList();
            recyclerView.SetAdapter(selectDurationRecyclerAdapter);

            selectDurationRecyclerAdapter.ItemTapObservable
                .Subscribe(ViewModel.SelectDurationFormat.Inputs)
                .DisposedBy(DisposeBag);

            return view;
        }

        public override void OnResume()
        {
            base.OnResume();

            Dialog.Window.SetDefaultDialogLayout(Activity, Context, heightDp: 268);
        }

        public override void OnCancel(IDialogInterface dialog)
        {
            ViewModel.Close.Execute();
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            if (!disposing) return;
            DisposeBag.Dispose();
        }
    }
}

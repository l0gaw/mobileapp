﻿using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;
using Toggl.Core.UI.ViewModels;
using Toggl.Droid.Adapters;
using Toggl.Droid.Views;

namespace Toggl.Droid.Fragments
{
    public partial class SelectColorFragment
    {
        private HueSaturationPickerView hueSaturationPicker;
        private ValueSlider valueSlider;
        private Button saveButton;
        private Button closeButton;
        private RecyclerView recyclerView;
        private SimpleAdapter<SelectableColorViewModel> selectableColorsAdapter;

        protected override void InitializeViews(View view) 
        {
            recyclerView = view.FindViewById<RecyclerView>(Resource.Id.SelectColorRecyclerView);
            saveButton = view.FindViewById<Button>(Resource.Id.SelectColorSave);
            closeButton = view.FindViewById<Button>(Resource.Id.SelectColorClose);
            hueSaturationPicker = view.FindViewById<HueSaturationPickerView>(Resource.Id.SelectColorHueSaturationPicker);
            valueSlider = view.FindViewById<ValueSlider>(Resource.Id.SelectColorValueSlider);
        }
    }
}

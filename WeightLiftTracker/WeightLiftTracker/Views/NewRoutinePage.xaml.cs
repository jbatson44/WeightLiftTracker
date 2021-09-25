using System;
using System.Collections.Generic;
using System.ComponentModel;
using WeightLiftTracker.Models;
using WeightLiftTracker.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace WeightLiftTracker.Views
{
    public partial class NewRoutinePage : ContentPage
    {
        public Routine Item { get; set; }

        public NewRoutinePage()
        {
            InitializeComponent();
            BindingContext = new NewRoutineViewModel();
        }
    }
}
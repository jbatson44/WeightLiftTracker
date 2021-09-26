using System;
using System.Collections.Generic;
using System.ComponentModel;
using WeightLiftTracker.Models;
using WeightLiftTracker.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace WeightLiftTracker.Views
{
    public partial class NewExercisePage : ContentPage
    {
        public Exercise Item { get; set; }

        public NewExercisePage()
        {
            InitializeComponent();
            BindingContext = new NewExerciseViewModel();
        }
    }
}
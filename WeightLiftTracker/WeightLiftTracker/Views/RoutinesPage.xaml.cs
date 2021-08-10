using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeightLiftTracker.Models;
using WeightLiftTracker.ViewModels;
using WeightLiftTracker.Views;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace WeightLiftTracker.Views
{
    public partial class RoutinesPage : ContentPage
    {
        RoutinesViewModel _viewModel;

        public RoutinesPage()
        {
            InitializeComponent();

            BindingContext = _viewModel = new RoutinesViewModel();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            _viewModel.OnAppearing();
        }
    }
}
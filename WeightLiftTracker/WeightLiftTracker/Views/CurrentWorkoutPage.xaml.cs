using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeightLiftTracker.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace WeightLiftTracker.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CurrentWorkoutPage : ContentPage
    {
        public CurrentWorkoutPage()
        {
            InitializeComponent();
            BindingContext = new CurrentWorkoutViewModel();
        }

        private void Entry_Focused(object sender, FocusEventArgs e)
        {
            Entry entry = (Entry)sender;
            entry.Text = "";
        }
    }
}
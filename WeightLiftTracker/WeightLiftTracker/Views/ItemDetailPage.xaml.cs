using System.ComponentModel;
using WeightLiftTracker.ViewModels;
using Xamarin.Forms;

namespace WeightLiftTracker.Views
{
    public partial class ItemDetailPage : ContentPage
    {
        public ItemDetailPage()
        {
            InitializeComponent();
            BindingContext = new ItemDetailViewModel();
        }
    }
}
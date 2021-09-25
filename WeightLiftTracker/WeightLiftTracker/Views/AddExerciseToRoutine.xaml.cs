using WeightLiftTracker.ViewModels;
using Xamarin.Forms;

namespace WeightLiftTracker.Views
{
    public partial class AddExerciseToRoutine : ContentPage
    {
        public AddExerciseToRoutine()
        {
            InitializeComponent();
            BindingContext = new AddExerciseViewModel();
        }
    }
}
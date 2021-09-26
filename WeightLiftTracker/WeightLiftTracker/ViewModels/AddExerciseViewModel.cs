using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using WeightLiftTracker.Models;
using Xamarin.Forms;

namespace WeightLiftTracker.ViewModels
{
    public class AddExerciseViewModel : BaseViewModel
    {
        private string name;
        public string Name
        {
            get => name;
            set => SetProperty(ref name, value);
        }
        public List<Exercise> Exercises { get; set; }

        public AddExerciseViewModel()
        {
            Exercises = new List<Exercise>();
            LoadExercises();
            SaveCommand = new Command(OnSave, ValidateSave);
            CancelCommand = new Command(OnCancel);
            this.PropertyChanged +=
                (_, __) => SaveCommand.ChangeCanExecute();
        }
        async Task LoadExercises()
        {
            try
            {
                Exercises = await App.Database.GetAllExercises();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
        }

        private bool ValidateSave()
        {
            return !String.IsNullOrWhiteSpace(name);
        }

        public Command SaveCommand { get; }
        public Command CancelCommand { get; }

        private async void OnCancel()
        {
            // This will pop the current page off the navigation stack
            await Shell.Current.GoToAsync("..");
        }

        private async void OnSave()
        {
            Routine routine = new Routine()
            {
                Id = 1,
                Name = name
            };

            await App.Database.SaveRoutineAsync(routine);

            // This will pop the current page off the navigation stack
            await Shell.Current.GoToAsync("..");
        }
    }
}

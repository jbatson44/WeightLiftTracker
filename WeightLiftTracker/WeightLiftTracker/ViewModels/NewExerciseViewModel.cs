using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using WeightLiftTracker.Models;
using Xamarin.Forms;

namespace WeightLiftTracker.ViewModels
{
    public class ExerciseCategory
    {
        public string Name { get; set; }
    }
    public class NewExerciseViewModel : BaseViewModel
    {
        private string name;
        private List<ExerciseCategory> _exerciseCategories;
        private ExerciseCategory _selectedCategory;
        public string Name
        {
            get => name;
            set => SetProperty(ref name, value);
        }
        public List<ExerciseCategory> ExerciseCategories 
        {
            get => _exerciseCategories;
            set => SetProperty(ref _exerciseCategories, value);
        }
        public ExerciseCategory SelectedCategory
        {
            get => _selectedCategory;
            set => SetProperty(ref _selectedCategory, value);
        }
        public NewExerciseViewModel()
        {
            ExerciseCategories = new List<ExerciseCategory>();
            ExerciseCategories.Add(new ExerciseCategory
            {
                Name = "Chest"
            });
            SaveCommand = new Command(OnSave, ValidateSave);
            CancelCommand = new Command(OnCancel);
            PropertyChanged +=
                (_, __) => SaveCommand.ChangeCanExecute();
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
            Exercise exercise = new Exercise()
            {
                Id = 1,
                Name = name,
                Category = SelectedCategory.Name
            };

            await App.Database.SaveExerciseAsync(exercise);

            // This will pop the current page off the navigation stack
            await Shell.Current.GoToAsync("..");
        }
    }
}

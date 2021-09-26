using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using WeightLiftTracker.Models;
using WeightLiftTracker.Views;
using Xamarin.Forms;

namespace WeightLiftTracker.ViewModels
{
    public class ExercisesListViewModel : BaseViewModel
    {
        private Exercise _selectedExercise;

        public ObservableCollection<Exercise> Exercises { get; }
        public Command LoadExercisesCommand { get; }
        public Command AddExerciseCommand { get; }
        public Command<Exercise> ItemTapped { get; }
        public Command<Exercise> DeleteExerciseCommand { get; }

        public ExercisesListViewModel()
        {
            Title = "Exercises";
            Exercises = new ObservableCollection<Exercise>();
            LoadExercisesCommand = new Command(async () => await ExecuteLoadItemsCommand());

            ItemTapped = new Command<Exercise>(OnItemSelected);
            DeleteExerciseCommand = new Command<Exercise>(DeleteExercise);

            AddExerciseCommand = new Command(OnAddItem);
        }

        async Task ExecuteLoadItemsCommand()
        {
            IsBusy = true;

            try
            {
                Exercises.Clear();
                var exercises = await App.Database.GetAllExercises();
                foreach (var exercise in exercises)
                {
                    Exercises.Add(exercise);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
            finally
            {
                IsBusy = false;
            }
        }

        public void OnAppearing()
        {
            IsBusy = true;
            SelectedItem = null;
        }

        public Exercise SelectedItem
        {
            get => _selectedExercise;
            set
            {
                SetProperty(ref _selectedExercise, value);
                OnItemSelected(value);
            }
        }

        private async void OnAddItem(object obj)
        {
            await Shell.Current.GoToAsync(nameof(NewExercisePage));
        }

        async void OnItemSelected(Exercise exercise)
        {
            if (exercise == null)
                return;

            // This will push the ItemDetailPage onto the navigation stack
            //await Shell.Current.GoToAsync($"{nameof(ItemDetailPage)}?routineId={exercise.Id}");
        }

        async void DeleteExercise(Exercise exercise)
        {
            if (exercise == null)
                return;

            IsBusy = true;

            try
            {
                var rows = await App.Database.DeleteExercise(exercise);
                if (rows > 0)
                {
                    Exercises.Remove(Exercises.SingleOrDefault(i => i.Id == exercise.Id));
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}
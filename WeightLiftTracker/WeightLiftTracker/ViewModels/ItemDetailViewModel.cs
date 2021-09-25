using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;
using WeightLiftTracker.Models;
using WeightLiftTracker.Views;
using Xamarin.Forms;

namespace WeightLiftTracker.ViewModels
{
    [QueryProperty(nameof(RoutineId), "routineId")]
    public class ItemDetailViewModel : BaseViewModel
    {
        private Exercise _selectedExercise;

        public ObservableCollection<Exercise> Exercises { get; set; }
        public Command LoadExercisesCommand { get; }
        public Command AddExerciseCommand { get; }
        public Command<Routine> ItemTapped { get; }
        public string RoutineId
        {
            set
            {
                LoadEverything(value);
            }
        }
        public Routine Routine { get; set; }

        public async void LoadEverything(string routineId)
        {
            Routine = await App.Database.GetRoutineById(int.Parse(routineId));
            Title = Routine.Name;
        }

        public ItemDetailViewModel()
        {
            Title = "None";
            Exercises = new ObservableCollection<Exercise>();
            LoadExercisesCommand = new Command(async () => await ExecuteLoadItemsCommand());

            //ItemTapped = new Command<Routine>(OnItemSelected);

            AddExerciseCommand = new Command(OnAddItem);
        }

        async Task ExecuteLoadItemsCommand()
        {
            IsBusy = true;

            try
            {
                Exercises.Clear();
                var exercises = await App.Database.GetExercisesByRoutine(Routine.Id);
                foreach (var exercise in Exercises)
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
            SelectedExercise = null;
        }

        public Exercise SelectedExercise
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
            await Shell.Current.GoToAsync(nameof(NewRoutinePage));
        }

        async void OnItemSelected(Exercise exercise)
        {
            if (exercise == null)
                return;

            // This will push the ItemDetailPage onto the navigation stack
            //await Shell.Current.GoToAsync($"{nameof(ItemDetailPage)}?{nameof(ItemDetailViewModel.ItemId)}={exercise.Id}");
        }
    }
}
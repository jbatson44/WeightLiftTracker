using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using WeightLiftTracker.Models;
using Xamarin.Forms;

namespace WeightLiftTracker.ViewModels
{
    [QueryProperty(nameof(RoutineId), "routineId")]
    public class CurrentWorkoutViewModel : BaseViewModel
    {
        public ObservableCollection<WorkoutExercise> Exercises { get; set; }
        public string RoutineId
        {
            set => LoadEverything(value);
        }
        public Routine Routine { get; set; }

        public async void LoadEverything(string routineId)
        {
            Routine = await App.Database.GetRoutineById(int.Parse(routineId));
            Title = Routine.Name;
            IsBusy = true;
        }

        public Command LoadExercisesCommand { get; }
        public Command AddExerciseCommand { get; }
        public Command<WorkoutExercise> AddSetCommand { get; }
        public Command FinishWorkoutCommand { get; }
        public Command<WorkoutExercise> DeleteExerciseCommand { get; }
        public CurrentWorkoutViewModel()
        {
            Exercises = new ObservableCollection<WorkoutExercise>();
            LoadExercisesCommand = new Command(async () => await ExecuteLoadItemsCommand());
            DeleteExerciseCommand = new Command<WorkoutExercise>(DeleteExercise);
            AddSetCommand = new Command<WorkoutExercise>(AddSet);

            AddExerciseCommand = new Command(OnAddItem);
            FinishWorkoutCommand = new Command(FinishWorkout);
        }
        public async Task ExecuteLoadItemsCommand()
        {
            IsBusy = true;
            if (Routine == null)
            {
                IsBusy = false;
                return;
            }


            try
            {
                if (Exercises == null || Exercises.Count == 0)
                {
                    var exercises = await App.Database.GetExercisesByRoutine(Routine.Id);
                    foreach (var exercise in exercises)
                    {
                        var newEx = new WorkoutExercise(exercise.Id, exercise.Name, new ObservableCollection<WorkoutSet>
                    {
                        new WorkoutSet()
                    });
                        Exercises.Add(newEx);
                    }
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
        public void DeleteExercise(WorkoutExercise exercise)
        {
            if (exercise == null)
                return;

            IsBusy = true;

            try
            {
                Exercises.Remove(exercise);
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
        public void AddSet(WorkoutExercise exercise)
        {
            if (exercise == null && exercise.ExerciseId == 1)
                return;

            try
            {
                Exercises.FirstOrDefault(x => x.ExerciseId == exercise.ExerciseId).Add(new WorkoutSet());
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
        }

        public void OnAppearing()
        {
            IsBusy = true;
        }

        private async void OnAddItem(object obj)
        {
            //await Shell.Current.GoToAsync($"{nameof(AddExerciseToRoutine)}?routineId={Routine.Id}");
        }

        public void FinishWorkout()
        {

        }
    }
}

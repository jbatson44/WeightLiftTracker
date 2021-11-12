using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using WeightLiftTracker.Models;
using Xamarin.Forms;

namespace WeightLiftTracker.ViewModels
{
    [QueryProperty(nameof(WorkoutId), "workoutId")]
    public class PreviousWorkoutViewModel : BaseViewModel
    {
        public DateTime Date { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan? EndTime { get; set; }
        public ObservableCollection<WorkoutExercise> Exercises { get; set; }
        public string WorkoutId
        {
            set => LoadEverything(value);
        }
        public Workout Workout { get; set; }

        public async void LoadEverything(string workoutId)
        {
            Workout = await App.Database.GetWorkoutById(int.Parse(workoutId));
            StartTime = Workout.StartTime.TimeOfDay;
            EndTime = Workout.EndTime.TimeOfDay;
            Date = Workout.StartTime.Date;

            //Title = Worko;
            IsBusy = true;
        }

        public Command LoadExercisesCommand { get; }
        public Command AddExerciseCommand { get; }
        public Command<WorkoutExercise> AddSetCommand { get; }
        public Command FinishWorkoutCommand { get; }
        public Command<WorkoutExercise> DeleteExerciseCommand { get; }
        public PreviousWorkoutViewModel()
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
            if (Workout == null)
            {
                IsBusy = false;
                return;
            }

            try
            {
                if (Exercises == null || Exercises.Count == 0)
                {
                    var sets = await App.Database.GetSetsByWorkout(Workout.Id);
                    sets.Select(s => s.ExerciseId);
                    Exercises = new ObservableCollection<WorkoutExercise>();
                    foreach (var set in sets)
                    {
                        if (Exercises.Any(e => e.ExerciseId == set.ExerciseId))
                        {
                            Exercises.Where(e => e.ExerciseId == set.ExerciseId).FirstOrDefault().Add(
                                new WorkoutSet()
                                {
                                    Weight = set.Weight,
                                    Reps = set.Reps
                                }
                            );
                        }
                        else
                        {
                            ObservableCollection<WorkoutSet> ws = new ObservableCollection<WorkoutSet>
                        {
                            new WorkoutSet()
                            {
                                Weight = set.Weight,
                                Reps = set.Reps
                            }
                        };
                            WorkoutExercise we = new WorkoutExercise(set.ExerciseId, set.ExerciseName, ws);
                            Exercises.Add(we);
                        }
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

        public async void FinishWorkout()
        {
            //if (EndTime == null)
            //{
            //    EndTime = DateTime.Now.TimeOfDay;
            //}
            //Workout workout = new Workout
            //{
            //    StartTime = Date.Date + StartTime,
            //    EndTime = Date.Date + EndTime.GetValueOrDefault(),
            //    RoutineId = Routine.Id,
            //    RoutineName = Routine.Name,
            //    Duration = EndTime.GetValueOrDefault() - StartTime
            //};
            //int rows = await App.Database.SaveWorkoutAsync(workout);
            //if (rows > 0)
            //{
            //    foreach (var ex in Exercises)
            //    {
            //        for (int i = 0; i < ex.Count; i++)
            //        {
            //            if (ex[i].Reps > 0)
            //            {
            //                Set set = new Set
            //                {
            //                    SetNumber = i,
            //                    ExerciseId = ex.ExerciseId,
            //                    ExerciseName = ex.ExerciseName,
            //                    Reps = ex[i].Reps,
            //                    Weight = ex[i].Weight,
            //                    WorkoutId = workout.Id
            //                };
            //                rows = await App.Database.SaveSetAsync(set);
            //            }
            //        }
            //    }
            //}

            //await Shell.Current.GoToAsync("..");
        }
    }
}

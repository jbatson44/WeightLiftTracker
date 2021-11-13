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
        public DateTime Date { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan? EndTime { get; set; }
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
            await ExecuteLoadItemsCommand();
        }

        public Command LoadExercisesCommand { get; }
        public Command AddExerciseCommand { get; }
        public Command<WorkoutExercise> AddSetCommand { get; }
        public Command FinishWorkoutCommand { get; }
        public Command<WorkoutExercise> DeleteExerciseCommand { get; }
        public Command<WorkoutSet> DeleteSetCommand { get; }
        public CurrentWorkoutViewModel()
        {
            Date = DateTime.Today;
            StartTime = DateTime.Now.TimeOfDay;
            Exercises = new ObservableCollection<WorkoutExercise>();
            LoadExercisesCommand = new Command(async () => await ExecuteLoadItemsCommand());

            DeleteExerciseCommand = new Command<WorkoutExercise>(DeleteExercise);
            DeleteSetCommand = new Command<WorkoutSet>(DeleteSet);

            AddSetCommand = new Command<WorkoutExercise>(AddSet);
            AddExerciseCommand = new Command(OnAddItem);
            FinishWorkoutCommand = new Command(FinishWorkout);
        }
        public async Task ExecuteLoadItemsCommand()
        {
            if (Routine == null)
            {
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
                            new WorkoutSet(exercise.Name)
                            {
                                Reps = 0,
                                Weight = 0
                            }
                        });
                        Exercises.Add(newEx);
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
        }
        public void DeleteExercise(WorkoutExercise exercise)
        {
            if (exercise == null)
                return;

            try
            {
                Exercises.Remove(exercise);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
        }
        public void DeleteSet(WorkoutSet set)
        {
            if (set == null)
                return;

            try
            {
                var exercise = Exercises.FirstOrDefault(x=>x.ExerciseName == set.ExerciseName);
                exercise.Remove(exercise.FirstOrDefault(x => x.Id == set.Id));
                if (exercise.Count == 0)
                {
                    DeleteExercise(exercise);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
        }
        public void AddSet(WorkoutExercise exercise)
        {
            if (exercise == null && exercise.ExerciseId == 1)
                return;

            try
            {
                var newSet = new WorkoutSet(exercise.ExerciseName)
                {
                    Weight = 0,
                    Reps = 0
                };
                var ex = Exercises.FirstOrDefault(x => x.ExerciseId == exercise.ExerciseId);
                newSet.Id = ex.Count;
                ex.Add(newSet);
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
            if (EndTime == null)
            {
                EndTime = DateTime.Now.TimeOfDay;
            }
            Workout workout = new Workout
            {
                StartTime = Date.Date + StartTime,
                EndTime = Date.Date + EndTime.GetValueOrDefault(),
                RoutineId = Routine.Id,
                RoutineName = Routine.Name,
                Duration = EndTime.GetValueOrDefault() - StartTime
            };
            int rows = await App.Database.SaveWorkoutAsync(workout);
            if (rows > 0)
            {
                foreach (var ex in Exercises)
                {
                    for (int i = 0; i < ex.Count; i++)
                    {
                        if (ex[i].Reps > 0)
                        {
                            Set set = new Set
                            {
                                SetNumber = i,
                                ExerciseId = ex.ExerciseId,
                                ExerciseName = ex.ExerciseName,
                                Reps = ex[i].Reps,
                                Weight = ex[i].Weight,
                                WorkoutId = workout.Id
                            };
                            rows = await App.Database.SaveSetAsync(set);
                        }
                    }
                }
            }

            await Shell.Current.GoToAsync("..");
        }
    }
}

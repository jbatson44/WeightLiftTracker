using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace WeightLiftTracker.Models
{
    public class WorkoutExercise : ObservableCollection<WorkoutSet>
    {
        public int ExerciseId { get; set; }
        public string ExerciseName { get; set; }
        public WorkoutExercise(int id, string name, ObservableCollection<WorkoutSet> list) : base(list)
        {
            ExerciseId = id;
            ExerciseName = name;
        }
    }
    public class WorkoutSet
    {
        public int Reps { get; set; }
        public int Weight { get; set; }
    }
}

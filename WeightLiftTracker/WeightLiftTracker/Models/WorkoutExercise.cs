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
        public WorkoutSet(string name)
        {
            ExerciseName = name;
            Reps = null;
            Weight = null;
        }
        public int Id { get; set; }
        public string ExerciseName { get; set; }
        public int? Reps { get; set; }
        public int? Weight { get; set; }
        public int PrevReps { get; set; }
        public int PrevWeight { get; set; }
    }
}

using SQLite;
using SQLiteNetExtensions.Attributes;
using System;

namespace WeightLiftTracker.Models
{
    public class Set
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        [OneToOne("ExerciseId")]
        public Exercise Exercise { get; set; }
        [OneToOne("WorkoutId")]
        public Workout Workout { get; set; }
        public int Reps { get; set; }
        public int SetNumber { get; set; }
        public int Weight { get; set; }
    }
}

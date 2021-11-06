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
        public int ExerciseId { get; set; }
        public string ExerciseName { get; set; }
        [OneToOne("WorkoutId")]
        public int WorkoutId { get; set; }
        public int Reps { get; set; }
        public int SetNumber { get; set; }
        public int Weight { get; set; }
    }
}

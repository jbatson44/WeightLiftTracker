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
        public DateTime CreatedDate { get; set; }
        public int Reps { get; set; }
    }
}

using SQLite;
using SQLiteNetExtensions.Attributes;
using System;

namespace WeightLiftTracker.Models
{
    public class Workout
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        [OneToOne("RoutineId")]
        public Routine Routine { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
    }
}

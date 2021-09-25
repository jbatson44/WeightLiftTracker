using SQLite;
using SQLiteNetExtensions.Attributes;
using System.Collections.Generic;

namespace WeightLiftTracker.Models
{
    public class Exercise
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        [ManyToMany(typeof(Routine))]
        public List<Routine> Routines { get; set; }
        public string Name { get; set; }
        public int NumberOfSets { get; set; }
    }
}

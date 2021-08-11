using SQLite;
using System;

namespace WeightLiftTracker.Models
{
    public class Routine
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
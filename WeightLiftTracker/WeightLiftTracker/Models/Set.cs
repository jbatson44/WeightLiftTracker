using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace WeightLiftTracker.Models
{
    class Set
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public DateTime CreatedDate { get; set; }
        public int Reps { get; set; }
    }
}

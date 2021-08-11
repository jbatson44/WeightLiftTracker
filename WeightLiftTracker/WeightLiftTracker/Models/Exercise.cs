﻿using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace WeightLiftTracker.Models
{
    class Exercise
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string Name { get; set; }
        public int NumberOfSets { get; set; }
    }
}

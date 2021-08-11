using SQLite;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WeightLiftTracker.Models;

namespace WeightLiftTracker.Services
{
    public class WorkoutRepository
    {
        static SQLiteAsyncConnection database;

        public WorkoutRepository(string dbPath)
        {
            database = new SQLiteAsyncConnection(dbPath);
            database.CreateTableAsync<Routine>().Wait();
            database.CreateTableAsync<Exercise>().Wait();
            database.CreateTableAsync<Set>().Wait();
        }

        public Task<List<Routine>> GetAllRoutines()
        {
            return database.Table<Routine>().ToListAsync();
        }

        public Task<int> SaveRoutineAsync(Routine routine)
        {
            return database.InsertAsync(routine);
        }

        public Task<Routine> GetRoutineById(int id)
        {
            return database.GetAsync<Routine>(id);
        }
    }
}

﻿using SQLite;
using System.Collections.Generic;
using System.Linq;
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
            database.CreateTableAsync<RoutineExerciseGroups>().Wait();
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
        public Task<int> DeleteRoutine(Routine routine)
        {
            return database.DeleteAsync(routine);
        }
        public Task<int> DeleteExercise(Exercise exercise)
        {
            return database.DeleteAsync(exercise);
        }

        public Task<List<Exercise>> GetAllExercises()
        {
            return database.Table<Exercise>().ToListAsync();
        }

        public Task<int> SaveExerciseAsync(Exercise exercise)
        {
            return database.InsertAsync(exercise);
        }

        public Task<List<Exercise>> GetExercisesByRoutine(int routineId)
        {
            return database.QueryAsync<Exercise>(@"
SELECT * 
FROM Exercise e
JOIN RoutineExerciseGroups reg
ON e.Id = reg.ExerciseId
WHERE reg.RoutineId = ?
", routineId);
        }
        public Task<List<Exercise>> GetAllExercisesNotInRoutine(int routineId)
        {
            var exercises = GetExercisesByRoutine(routineId).Result.Select(x => x.Id).ToList();
            var list = CommaSeparatedList(exercises);
            string query = "SELECT * FROM Exercise WHERE Id NOT IN (" + list + ")";
            return database.QueryAsync<Exercise>(query);
        }
        public string CommaSeparatedList(List<int> ids)
        {
            string list = "";
            foreach(int i in ids)
            {
                if (list.Length > 0)
                {
                    list += ",";
                }
                list += i;
            }
            return list;
        }
        public Task AddExerciseToRoutine(int exerciseId, int routineId)
        {
            return database.InsertAsync(new RoutineExerciseGroups
            {
                ExerciseId = exerciseId,
                RoutineId = routineId
            });
        }
        public Task RemoveExerciseFromRoutine(int exerciseId, int routineId)
        {
            return database.QueryAsync<RoutineExerciseGroups>(@"
DELETE FROM RoutineExerciseGroups
WHERE ExerciseId = " + exerciseId +
" AND RoutineId = ?", routineId);
        }
    }
}

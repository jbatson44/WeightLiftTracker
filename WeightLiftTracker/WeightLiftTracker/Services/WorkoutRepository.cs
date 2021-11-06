using SQLite;
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
            database.CreateTableAsync<Workout>().Wait();
        }

        #region Get Methods
        public Task<List<Routine>> GetAllRoutines()
        {
            return database.Table<Routine>().ToListAsync();
        }

        public Task<Routine> GetRoutineById(int id)
        {
            return database.GetAsync<Routine>(id);
        }

        public Task<List<Exercise>> GetAllExercises()
        {
            return database.Table<Exercise>().ToListAsync();
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

        public Task<List<Workout>> GetAllWorkouts()
        {
            return database.Table<Workout>().ToListAsync();
        }
        #endregion

        #region Insert Methods
        public Task<int> SaveRoutineAsync(Routine routine)
        {
            return database.InsertAsync(routine);
        }

        public Task<int> SaveWorkoutAsync(Workout workout)
        {
            return database.InsertAsync(workout);
        }

        public Task<int> SaveSetAsync(Set set)
        {
            return database.InsertAsync(set);
        }

        public Task<int> SaveExerciseAsync(Exercise exercise)
        {
            return database.InsertAsync(exercise);
        }

        public Task AddExerciseToRoutine(int exerciseId, int routineId)
        {
            return database.InsertAsync(new RoutineExerciseGroups
            {
                ExerciseId = exerciseId,
                RoutineId = routineId
            });
        }
        #endregion

        #region Delete methods
        public Task<int> DeleteRoutine(Routine routine)
        {
            return database.DeleteAsync(routine);
        }

        public Task<int> DeleteExercise(Exercise exercise)
        {
            return database.DeleteAsync(exercise);
        }

        public Task RemoveExerciseFromRoutine(int exerciseId, int routineId)
        {
            return database.QueryAsync<RoutineExerciseGroups>(@"
DELETE FROM RoutineExerciseGroups
WHERE ExerciseId = " + exerciseId +
" AND RoutineId = ?", routineId);
        }

        public Task DeleteSetsFromWorkout(int workoutId)
        {
            return database.QueryAsync<Set>(@"
DELETE FROM [Set]
WHERE WorkoutId = ?", workoutId);
        }

        public Task<int> DeleteWorkout(Workout workout)
        {
            return database.DeleteAsync(workout);
        }
        #endregion

        #region Helper Methods
        public string CommaSeparatedList(List<int> ids)
        {
            string list = "";
            foreach (int i in ids)
            {
                if (list.Length > 0)
                {
                    list += ",";
                }
                list += i;
            }
            return list;
        }
        #endregion
    }
}

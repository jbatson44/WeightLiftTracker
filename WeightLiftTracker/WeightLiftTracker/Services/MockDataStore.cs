using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WeightLiftTracker.Models;

namespace WeightLiftTracker.Services
{
    public class MockDataStore : IDataStore<Routine>
    {
        readonly List<Routine> routines;

        public MockDataStore()
        {
            routines = new List<Routine>()
            {
                new Routine { Id = 1, Name = "First item" },
                new Routine { Id = 2, Name = "Second item" },
                new Routine { Id = 3, Name = "Third item" },
                new Routine { Id = 4, Name = "Fourth item" },
                new Routine { Id = 5, Name = "Fifth item" },
                new Routine { Id = 6, Name = "Sixth item" }
            };
        }

        public async Task<bool> AddItemAsync(Routine item)
        {
            item.Id = routines.LastOrDefault().Id + 1;
            routines.Add(item);

            return await Task.FromResult(true);
        }

        public async Task<bool> UpdateItemAsync(Routine item)
        {
            var oldItem = routines.Where((Routine arg) => arg.Id == item.Id).FirstOrDefault();
            routines.Remove(oldItem);
            routines.Add(item);

            return await Task.FromResult(true);
        }

        public async Task<bool> DeleteItemAsync(string id)
        {
            var oldItem = routines.Where((Routine arg) => arg.Id == int.Parse(id)).FirstOrDefault();
            routines.Remove(oldItem);

            return await Task.FromResult(true);
        }

        public async Task<Routine> GetItemAsync(string id)
        {
            return await Task.FromResult(routines.FirstOrDefault(s => s.Id == int.Parse(id)));
        }

        public async Task<IEnumerable<Routine>> GetItemsAsync(bool forceRefresh = false)
        {
            return await Task.FromResult(routines);
        }
    }
}
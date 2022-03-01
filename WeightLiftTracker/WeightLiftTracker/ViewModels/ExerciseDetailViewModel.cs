using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using WeightLiftTracker.Models;
using WeightLiftTracker.Views;
using Xamarin.Forms;

namespace WeightLiftTracker.ViewModels
{
    [QueryProperty(nameof(ExerciseId), "exerciseId")]
    public class ExerciseDetailViewModel : BaseViewModel
    {
        public string ExerciseId
        {
            set => LoadEverything(value);
        }
        public Exercise Exercise { get; set; }

        public async void LoadEverything(string exerciseId)
        {
            Exercise = await App.Database.GetExerciseById(int.Parse(exerciseId));
            Title = Exercise.Name;
            IsBusy = true;
        }
        public void OnAppearing()
        {
        }

        public ExerciseDetailViewModel()
        {
        }
    }
}
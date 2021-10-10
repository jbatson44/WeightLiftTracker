﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using WeightLiftTracker.Models;
using WeightLiftTracker.Views;
using Xamarin.Forms;

namespace WeightLiftTracker.ViewModels
{
    [QueryProperty(nameof(RoutineId), "routineId")]
    public class AddExerciseViewModel : BaseViewModel
    {
        private string name;
        public string Name
        {
            get => name;
            set => SetProperty(ref name, value);
        }
        private Exercise selectedExercise;
        public Exercise SelectedExercise
        {
            get => selectedExercise;
            set => SetProperty(ref selectedExercise, value);
        }
        public string RoutineId
        {
            set
            {
                LoadEverything(value);
            }
        }
        public Routine Routine { get; set; }
        public async void LoadEverything(string routineId)
        {
            Routine = await App.Database.GetRoutineById(int.Parse(routineId));
            LoadExercises();
        }
        private List<Exercise> exercises;
        public List<Exercise> Exercises 
        {
            get => exercises;
            set => SetProperty(ref exercises, value);
        }

        public AddExerciseViewModel()
        {
            SaveCommand = new Command(OnSave, ValidateSave);
            CancelCommand = new Command(OnCancel);
            PropertyChanged +=
                (_, __) => SaveCommand.ChangeCanExecute();
            NewExerciseCommand = new Command(OnAddItem);
        }

        public void LoadExercises()
        {
            try
            {
                Exercises = App.Database.GetAllExercisesNotInRoutine(Routine.Id).Result;
                SelectedExercise = Exercises.FirstOrDefault();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
        }

        private bool ValidateSave()
        {
            return SelectedExercise != null;
        }

        public Command SaveCommand { get; }
        public Command CancelCommand { get; }
        public Command NewExerciseCommand { get; }

        private async void OnCancel()
        {
            // This will pop the current page off the navigation stack
            await Shell.Current.GoToAsync("..");
        }
        private async void OnAddItem(object obj)
        {
            await Shell.Current.GoToAsync(nameof(NewExercisePage));
        }

        private async void OnSave()
        {
            await App.Database.AddExerciseToRoutine(SelectedExercise.Id, Routine.Id);

            // This will pop the current page off the navigation stack
            await Shell.Current.GoToAsync("..");
        }
    }
}

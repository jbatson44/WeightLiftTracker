﻿using System;
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
    [QueryProperty(nameof(RoutineId), "routineId")]
    public class RoutineDetailViewModel : BaseViewModel
    {
        private Exercise _selectedExercise;
        
        private List<Exercise> exercises;
        public List<Exercise> Exercises 
        {
            get => exercises;
            set => SetProperty(ref exercises, value);
        }
        public Command LoadExercisesCommand { get; }
        public Command AddExerciseCommand { get; }
        public Command<Routine> ItemTapped { get; }
        public Command<Exercise> DeleteExerciseCommand { get; }
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
            Title = Routine.Name;
            await ExecuteLoadItemsCommand();
        }

        public RoutineDetailViewModel()
        {
            Title = "None";
            Exercises = new List<Exercise>();
            LoadExercisesCommand = new Command(async () => await ExecuteLoadItemsCommand());
            DeleteExerciseCommand = new Command<Exercise>(DeleteExercise);

            //ItemTapped = new Command<Routine>(OnItemSelected);

            AddExerciseCommand = new Command(OnAddItem);
        }

        async Task ExecuteLoadItemsCommand()
        {
            IsBusy = true;

            try
            {
                Exercises = await App.Database.GetExercisesByRoutine(Routine.Id);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
            finally
            {
                IsBusy = false;
            }
        }
        async void DeleteExercise(Exercise exercise)
        {
            if (exercise == null)
                return;

            IsBusy = true;

            try
            {
                await App.Database.RemoveExerciseFromRoutine(exercise.Id, Routine.Id);
                Exercises.Remove(Exercises.SingleOrDefault(i => i.Id == exercise.Id));
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
            finally
            {
                IsBusy = false;
            }
        }

        public void OnAppearing()
        {
            IsBusy = true;
            SelectedExercise = null;
        }

        public Exercise SelectedExercise
        {
            get => _selectedExercise;
            set
            {
                SetProperty(ref _selectedExercise, value);
                OnItemSelected(value);
            }
        }

        private async void OnAddItem(object obj)
        {
            await Shell.Current.GoToAsync($"{nameof(AddExerciseToRoutine)}?routineId={Routine.Id}");
        }

        async void OnItemSelected(Exercise exercise)
        {
            if (exercise == null)
                return;

            // This will push the ItemDetailPage onto the navigation stack
            //await Shell.Current.GoToAsync($"{nameof(ItemDetailPage)}?{nameof(ItemDetailViewModel.ItemId)}={exercise.Id}");
        }
    }
}
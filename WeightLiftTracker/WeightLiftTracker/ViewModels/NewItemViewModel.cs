using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using WeightLiftTracker.Models;
using Xamarin.Forms;

namespace WeightLiftTracker.ViewModels
{
    public class NewItemViewModel : BaseViewModel
    {
        private string name;

        public NewItemViewModel()
        {
            SaveCommand = new Command(OnSave, ValidateSave);
            CancelCommand = new Command(OnCancel);
            this.PropertyChanged +=
                (_, __) => SaveCommand.ChangeCanExecute();
        }

        private bool ValidateSave()
        {
            return !String.IsNullOrWhiteSpace(name);
        }

        public string Name
        {
            get => name;
            set => SetProperty(ref name, value);
        }

        public Command SaveCommand { get; }
        public Command CancelCommand { get; }

        private async void OnCancel()
        {
            // This will pop the current page off the navigation stack
            await Shell.Current.GoToAsync("..");
        }

        private async void OnSave()
        {
            Routine routine = new Routine()
            {
                Id = 1,
                Name = name
            };

            await DataStore.AddItemAsync(routine);

            // This will pop the current page off the navigation stack
            await Shell.Current.GoToAsync("..");
        }
    }
}

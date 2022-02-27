using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using WeightLiftTracker.Models;
using WeightLiftTracker.Views;
using Xamarin.Forms;

namespace WeightLiftTracker.ViewModels
{
    public class RoutinesViewModel : BaseViewModel
    {
        private Routine _selectedRoutine;

        public ObservableCollection<Routine> Routines { get; }
        public Command LoadRoutinesCommand { get; }
        public Command AddRoutineCommand { get; }
        public Command<Routine> ItemTapped { get; }
        public Command<Routine> DeleteRoutineCommand { get; }

        public RoutinesViewModel()
        {
            Title = "Routines";
            Routines = new ObservableCollection<Routine>();
            LoadRoutinesCommand = new Command(async () => await ExecuteLoadItemsCommand());

            ItemTapped = new Command<Routine>(OnItemSelected);
            DeleteRoutineCommand = new Command<Routine>(DeleteRoutine);

            AddRoutineCommand = new Command(OnAddItem);
        }

        async Task ExecuteLoadItemsCommand()
        {
            IsBusy = true;

            try
            {
                Routines.Clear();
                var routines = await App.Database.GetAllRoutines();
                foreach (var routine in routines)
                {
                    Routines.Add(routine);
                }
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
            SelectedItem = null;
        }

        public Routine SelectedItem
        {
            get => _selectedRoutine;
            set
            {
                SetProperty(ref _selectedRoutine, value);
                OnItemSelected(value);
            }
        }

        private async void OnAddItem(object obj)
        {
            await Shell.Current.GoToAsync(nameof(NewRoutinePage));
        }

        async void OnItemSelected(Routine routine)
        {
            if (routine == null)
                return;

            await Shell.Current.GoToAsync($"{nameof(ItemDetailPage)}?routineId={routine.Id}");
        }

        async void DeleteRoutine(Routine routine)
        {
            if (routine == null)
                return;

            try
            {
                var rows = await App.Database.DeleteRoutine(routine);
                if (rows > 0)
                {
                    Routines.Remove(Routines.SingleOrDefault(i => i.Id == routine.Id));
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
        }
    }
}
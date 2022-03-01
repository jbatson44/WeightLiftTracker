using System;
using System.Collections.Generic;
using WeightLiftTracker.ViewModels;
using WeightLiftTracker.Views;
using Xamarin.Forms;

namespace WeightLiftTracker
{
    public partial class AppShell : Xamarin.Forms.Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute(nameof(RoutineDetailPage), typeof(RoutineDetailPage));
            Routing.RegisterRoute(nameof(ExerciseDetailPage), typeof(ExerciseDetailPage));
            Routing.RegisterRoute(nameof(AddExerciseToRoutine), typeof(AddExerciseToRoutine));
            Routing.RegisterRoute(nameof(NewRoutinePage), typeof(NewRoutinePage));
            Routing.RegisterRoute(nameof(NewExercisePage), typeof(NewExercisePage));
            Routing.RegisterRoute(nameof(CurrentWorkoutPage), typeof(CurrentWorkoutPage));
            Routing.RegisterRoute(nameof(PreviousWorkoutPage), typeof(PreviousWorkoutPage));
        }
    }
}

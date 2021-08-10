using System;
using WeightLiftTracker.Services;
using WeightLiftTracker.Views;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace WeightLiftTracker
{
    public partial class App : Application
    {

        public App()
        {
            InitializeComponent();

            DependencyService.Register<MockDataStore>();
            MainPage = new AppShell();
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}

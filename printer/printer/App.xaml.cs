using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using FreshMvvm;
[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace printer
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            var page = FreshPageModelResolver.ResolvePageModel<MainPageModel>();
            var container = new FreshNavigationContainer(page);
            MainPage = container;
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}

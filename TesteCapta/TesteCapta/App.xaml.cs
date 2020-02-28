using Prism;
using Prism.Ioc;
using System;
using System.IO;
using TesteCapta.Data;
using TesteCapta.ViewModels;
using TesteCapta.Views;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace TesteCapta
{
    public partial class App
    {

        public static MoedaDataBase moedaDataBase;

        public static MoedaDataBase DataBase
        {
            get
            {
                if (moedaDataBase == null)
                {
                    moedaDataBase = new MoedaDataBase(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                        "Moedas.db3"));
                }

                return moedaDataBase;
            }
        }
        /* 
         * The Xamarin Forms XAML Previewer in Visual Studio uses System.Activator.CreateInstance.
         * This imposes a limitation in which the App class must have a default constructor. 
         * App(IPlatformInitializer initializer = null) cannot be handled by the Activator.
         */
        public App() : this(null) { }

        public App(IPlatformInitializer initializer) : base(initializer) { }

        protected override async void OnInitialized()
        {
            InitializeComponent();

            await NavigationService.NavigateAsync("NavigationPage/MainPage");
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<NavigationPage>();
            containerRegistry.RegisterForNavigation<MainPage, MainPageViewModel>();
            containerRegistry.RegisterForNavigation<SelecionarMoedaPage, SelecionarMoedaPageViewModel>();
        }
    }
}

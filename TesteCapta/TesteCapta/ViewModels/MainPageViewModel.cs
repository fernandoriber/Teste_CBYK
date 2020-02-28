using Acr.UserDialogs;
using Prism.Commands;
using Prism.Navigation;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using TesteCapta.Helpers;
using TesteCapta.Models;

namespace TesteCapta.ViewModels
{
    public class MainPageViewModel : ViewModelBase
    {
        private MoedaDataBaseModel moedaSelecionada;

        public MoedaDataBaseModel MoedaSelecionada
        {
            get { return moedaSelecionada; }
            set { SetProperty(ref moedaSelecionada, value); }
        }

        private String txtCodigo;

        public String TxtCodigo
        {
            get { return txtCodigo; }
            set { SetProperty(ref txtCodigo, value); }
        }

        public ObservableCollection<MoedaDataBaseModel> MoedasBdCollection { get; set; }

        public DelegateCommand OpenPopUpCommand { get; set; }

        public DelegateCommand DeleteCommand { get; set; }
        
        public DelegateCommand SearchCommand { get; set; }

        private readonly INavigationService _navigationService;

        public MainPageViewModel(INavigationService navigationService)
            : base(navigationService)
        {
            Title = "Menu Principal";

            this._navigationService = navigationService;
            this.OpenPopUpCommand = new DelegateCommand(async () => await OpenPopUp());
            this.DeleteCommand = new DelegateCommand(async () => await DeleteMoeda());
            this.SearchCommand = new DelegateCommand(async () => await SelectMoeda());
            Task.Run(async () => { await GetMoedasBd(); });
        }

        async Task OpenPopUp()
        {
            await _navigationService.NavigateAsync("SelecionarMoedaPage");
        }

        public void SelectedMoedaBd(object selectedItem)
        {
            MoedaDataBaseModel moeda = new MoedaDataBaseModel();

            moeda = selectedItem as MoedaDataBaseModel;

            if (moeda != null)
            {
                MoedaSelecionada = moeda;
            }
        }

        async Task GetMoedasBd()
        {
            try
            {
                UserDialogs.Instance.ShowLoading("Obtendo informações do banco local...");

                var moedasList = await App.DataBase.GetMoedaAsync();

                if (moedasList.Count > 0)
                {
                    MoedasBdCollection = new ObservableCollection<MoedaDataBaseModel>(moedasList);
                    RaisePropertyChanged(nameof(MoedasBdCollection));
                }
                else
                {
                    MoedasBdCollection = new ObservableCollection<MoedaDataBaseModel>(moedasList);
                    RaisePropertyChanged(nameof(MoedasBdCollection));

                    UserDialogs.Instance.HideLoading();
                    await UserDialogs.Instance.AlertAsync("Não existem moedas salvas no dispositivo (SQLite).", Constants.AppName, "OK");
                    return;
                }
            }
            catch (Exception ex)
            {
                UserDialogs.Instance.HideLoading();
                await UserDialogs.Instance.AlertAsync("Erro ao obter informações do banco local." + ex.Message, Constants.AppName, "OK");
                return;
            }

            UserDialogs.Instance.HideLoading();
        }

        async Task DeleteMoeda()
        {
            try
            {
                UserDialogs.Instance.ShowLoading("Excluindo moeda do banco local...");

                if (MoedaSelecionada != null)
                {
                    int deletedRow = await App.DataBase.DeleteMoedaAsync(MoedaSelecionada);

                    if (deletedRow > 0)
                    {
                        UserDialogs.Instance.HideLoading();
                        await UserDialogs.Instance.AlertAsync("Moeda excluída com sucesso (SQLite).", Constants.AppName, "OK");

                        await GetMoedasBd();

                        MoedaSelecionada = null;
                    }
                }
                else
                {
                    UserDialogs.Instance.HideLoading();
                    await UserDialogs.Instance.AlertAsync("Selecione uma moeda para excluir (SQLite).", Constants.AppName, "OK");
                }
            }
            catch (Exception ex)
            {
                UserDialogs.Instance.HideLoading();
                await UserDialogs.Instance.AlertAsync("Erro ao obter informações do banco local." + ex.Message, Constants.AppName, "OK");
                return;
            }

            UserDialogs.Instance.HideLoading();
        }

        async Task SelectMoeda()
        {
            try
            {
                if (String.IsNullOrEmpty(TxtCodigo))
                {
                    UserDialogs.Instance.HideLoading();
                    await UserDialogs.Instance.AlertAsync("Informe um código para pesquisa.", Constants.AppName, "OK");
                    return;
                }
                else
                {
                    MoedaDataBaseModel moeda = await App.DataBase.GetMoedaAsync(TxtCodigo);

                    if(moeda != null)
                    {
                        UserDialogs.Instance.HideLoading();
                        await UserDialogs.Instance.AlertAsync("A moeda pesquisada foi " + moeda.NomeFormatado + ".", Constants.AppName, "OK");
                        return;
                    }
                    else
                    {
                        UserDialogs.Instance.HideLoading();
                        await UserDialogs.Instance.AlertAsync("Moeda não localizada no dispositivo.", Constants.AppName, "OK");
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                UserDialogs.Instance.HideLoading();
                await UserDialogs.Instance.AlertAsync("Erro ao obter informações do banco local." + ex.Message, Constants.AppName, "OK");
                return;
            }           
        }

        public override async void OnNavigatedTo(INavigationParameters parameters)
        {
            if (parameters.ContainsKey("MoedaSelecionada"))
            {
                if ((bool)parameters["MoedaSelecionada"])
                {
                    await GetMoedasBd();
                }
            }
        }
    }
}

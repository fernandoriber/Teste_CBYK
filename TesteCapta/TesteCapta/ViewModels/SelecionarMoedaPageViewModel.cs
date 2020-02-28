using Acr.UserDialogs;
using Prism.Commands;
using Prism.Navigation;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using TesteCapta.Helpers;
using TesteCapta.Models;
using TesteCapta.Services;

namespace TesteCapta.ViewModels
{
    public class SelecionarMoedaPageViewModel : ViewModelBase
    {
        private Value moedaSelecionada;

        public Value MoedaSelecionada
        {
            get { return moedaSelecionada; }
            set { SetProperty(ref moedaSelecionada, value); }
        }

        public ObservableCollection<Value> MoedasCollection { get; set; }

        public DelegateCommand SaveCommand { get; set; }

        private readonly INavigationService _navigationService;

        public SelecionarMoedaPageViewModel(INavigationService navigationService)
            : base(navigationService)
        {
            this._navigationService = navigationService;
            this.SaveCommand = new DelegateCommand(async () => await SaveMoeda());

            Task.Run(async () => { await GetMoedasList(); });
        }

        async Task GetMoedasList()
        {
            try
            {
                UserDialogs.Instance.ShowLoading("Obtendo dados do servidor...");

                if (!Utils.IsConnected())
                {
                    UserDialogs.Instance.HideLoading();
                    await UserDialogs.Instance.AlertAsync(Constants.NotConnected, Constants.AppName, "OK");
                    return;
                }
                else
                {
                    MoedaServices moedaServices = new MoedaServices();

                    var moedasList = await moedaServices.ConsultaMoedasAsync(100);

                    if (moedasList.Values.Count > 0)
                    {
                        MoedasCollection = new ObservableCollection<Value>(moedasList.Values);
                        RaisePropertyChanged(nameof(MoedasCollection));
                    }
                    else
                    {
                        UserDialogs.Instance.HideLoading();
                        await UserDialogs.Instance.AlertAsync("Não existem dados para exibição.", Constants.AppName, "OK");
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                UserDialogs.Instance.HideLoading();
                await UserDialogs.Instance.AlertAsync("Erro ao realizar a consulta." + ex.Message, Constants.AppName, "OK");
                return;
            }

            UserDialogs.Instance.HideLoading();
        }

        public void SelectedMoeda(object selectedItem)
        {
            Value moeda = new Value();

            moeda = selectedItem as Value;

            if (moeda != null)
            {
                MoedaSelecionada = moeda;
            }
        }

        async Task SaveMoeda()
        {
            try
            {
                if (MoedaSelecionada != null)
                {
                    MoedaDataBaseModel saveMoeda = new MoedaDataBaseModel();
                    saveMoeda.NomeFormatado = MoedaSelecionada.NomeFormatado;
                    saveMoeda.Simbolo = MoedaSelecionada.Simbolo;
                    saveMoeda.TipoMoeda = MoedaSelecionada.TipoMoeda;
                    saveMoeda.Date = DateTime.UtcNow;

                    var moedasList = await App.DataBase.GetMoedaAsync();

                    if(moedasList.Count == 0)
                    {
                        await App.DataBase.SaveMoedaAsync(saveMoeda);
                    }
                    else 
                    {
                        //Valida se a moeda já existe no BD
                        bool exists = false;

                        foreach (var i in moedasList)
                        {
                            if (MoedaSelecionada.Simbolo == i.Simbolo)
                            {
                                exists = true;
                            }
                        }

                        if (exists)
                        {
                            await UserDialogs.Instance.AlertAsync("Essa moeda já existe no banco de dados local.", Constants.AppName, "OK");
                            return;
                        }
                        else
                        {
                            await App.DataBase.SaveMoedaAsync(saveMoeda);
                        }
                    }
                }
            }
            catch (Exception)
            {
                await UserDialogs.Instance.AlertAsync("Erro ao salvar os dados.", Constants.AppName, "OK");
            }

            var parameters = new NavigationParameters();
            parameters.Add("MoedaSelecionada", true);
            await _navigationService.GoBackAsync(parameters);
        }
    }
}

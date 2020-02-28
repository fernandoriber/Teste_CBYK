using Rg.Plugins.Popup.Pages;
using TesteCapta.ViewModels;
using Xamarin.Forms;

namespace TesteCapta.Views
{
    public partial class SelecionarMoedaPage : PopupPage
    {
        public SelecionarMoedaPage()
        {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);
        }

        private void MoedasListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            ((SelecionarMoedaPageViewModel)BindingContext)?.SelectedMoeda(MoedasListView.SelectedItem);
        }

        protected override bool OnBackgroundClicked()
        {
            return false;
        }
    }
}

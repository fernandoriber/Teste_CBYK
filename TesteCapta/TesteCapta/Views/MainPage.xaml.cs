using TesteCapta.ViewModels;
using Xamarin.Forms;

namespace TesteCapta.Views
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        private void MoedasBdListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            ((MainPageViewModel)BindingContext)?.SelectedMoedaBd(MoedasBdListView.SelectedItem);
        }

        private void EntryCodigo_TextChanged(object sender, TextChangedEventArgs e)
        {
            var entry = sender as Entry;
            var text = entry.Text.ToUpper();

            if (entry.Text != text)
                entry.Text = text;
        }
    }
}
using MedicacaoDiariaApp.Models;
using MedicacaoDiariaApp.Helpers;
using System.Collections.ObjectModel;

namespace MedicacaoDiariaApp.Views;

public partial class ListaMedicamento : ContentPage
{
    ObservableCollection<Medicamento> lista = new ObservableCollection<Medicamento>();
	public ListaMedicamento()
	{
		InitializeComponent();

        lst_medicamentos.ItemsSource = lista;
	}

    protected async override void OnAppearing()
    {
        try
        {
            lista.Clear();

            List<Medicamento> tmp = await App.BancoDeDados.ListarMedicamento();
            tmp.ForEach(i => ListaMedicamento.Add(i));
        }
        catch (Exception ex)
        {
            await DisplayAlert("Ops", ex.Message, "OK");
        }
    }

    private static void Add(Medicamento i)
    {
        throw new NotImplementedException();
    }

    private void ToolbarItem_Clicked(object sender, EventArgs e)
    {
        try
        {
            Navigation.PushAsync(new Views.NovoMedicamento());
        }
        catch (Exception ex)
        {
            DisplayAlert("Ops", ex.Message, "OK");
        }        
    }

    private async void txt_search_TextChanged(object sender, TextChangedEventArgs e)
    {
        try
        {
            string q = e.NewTextValue;
            
            lst_medicamentos.IsRefreshing = true;

            lista.Clear();

            List<Medicamento> tmp = await App.BancoDeDados.Search(q);

            tmp.ForEach(i => lista.Add(i));
        }
        catch (Exception ex)
        {
            await DisplayAlert("Ops", ex.Message, "OK");
        }
        finally
        {
            lst_medicamentos.IsRefreshing = false;
        }
    }

    private void lst_medicamentos_ItemSelected(object sender, SelectedItemChangedEventArgs e)
    {
        try
        {
            Medicamento med = e.SelectedItem as Medicamento;

            Navigation.PushAsync(new Views.EditarMedicamento
            {
                BindingContext = med,
            });
        }
        catch (Exception ex)
        {
            DisplayAlert("Ops", ex.Message, "OK");
        }
    }

    private async void lst_medicamentos_Refreshing(object sender, SelectedItemChangedEventArgs e)
    {
        try
        {
            lista.Clear();

            List<Medicamento> tmp = await App.BancoDeDados.ListarMedicamento();

            tmp.ForEach(i => lista.Add(i));
        }
        catch (Exception ex)
        {
            await DisplayAlert("Ops", ex.Message, "OK");

        }
        finally
        {
            lst_medicamentos.IsRefreshing = false;
        }
    }

    private async void MenuItem_Clicked(object sender, EventArgs e)
    {
        try
        {
            MenuItem selecionado = sender as MenuItem;
            
            Medicamento med = selecionado.BindingContext as Medicamento;

            bool confirm = await DisplayAlert(
                "Tem Certeza?", $"Remover {med.Nome}?", "Sim", "Não");

            if(confirm)
            {
                await App.BancoDeDados.ExcluirMedicamento(med.IdMedicamento);
                lista.Remove(med);
            }
        }
        catch (Exception ex)
        {
            DisplayAlert("Ops", ex.Message, "OK");
        }
    }
}
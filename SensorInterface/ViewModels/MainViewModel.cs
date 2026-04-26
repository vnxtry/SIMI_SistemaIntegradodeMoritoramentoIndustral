using SensorInterface.Commands;
using Shared;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Net.Http.Json;
using System.Windows.Input;

namespace SensorInterface.ViewModels
{
    internal class MainViewModel : BaseViewModel
    {
        // Agora a lista guarda o objeto completo (Temperatura, Pressão, etc.)
        public ObservableCollection<SensorData> ListaSensores { get; set; }

        public ICommand CarregarSensoresCommand { get; }

        public MainViewModel()
        {
            ListaSensores = new ObservableCollection<SensorData>();
            CarregarSensoresCommand = new RelayCommand(CarregarSensores);
        }

        private async void CarregarSensores()
        {
            try
            {
                var http = new HttpClient();
                var dados = await http.GetFromJsonAsync<List<SensorData>>(
                    "https://localhost:44379/api/v1/sensores");

                if (dados != null)
                {
                    ListaSensores.Clear();

                    foreach (var registro in dados)
                    {
                        // Adiciona o objeto completo na lista da tela
                        ListaSensores.Add(registro);
                    }
                }
            }
            catch (Exception ex)
            {
                // Opcional: Adicionar um log de erro ou MessageBox aqui
                Console.WriteLine($"Erro ao carregar dados: {ex.Message}");
            }
        }
    }
}
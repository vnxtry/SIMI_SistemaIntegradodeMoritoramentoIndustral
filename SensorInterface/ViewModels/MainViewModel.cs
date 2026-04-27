using SensorInterface.Commands;
using Shared;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net.Http;
using System.Net.Http.Json;
using System.Windows; // Necessário para o MessageBox
using System.Windows.Input;

namespace SensorInterface.ViewModels
{
    internal class MainViewModel : BaseViewModel
    {
        public ObservableCollection<SensorData> ListaSensores { get; set; }

        // 1. Nova propriedade: Guarda o valor digitado na tela de configuração
        private double _temperaturaMaxima;
        public double TemperaturaMaxima
        {
            get { return _temperaturaMaxima; }
            set
            {
                _temperaturaMaxima = value;
                OnPropertyChanged(nameof(TemperaturaMaxima)); // Atualiza a tela em tempo real
            }
        }

        public ICommand CarregarSensoresCommand { get; }

        // 2. Novo comando: Vai ser "amarrado" ao botão Salvar do WPF
        public ICommand SalvarConfiguracaoCommand { get; }

        public MainViewModel()
        {
            ListaSensores = new ObservableCollection<SensorData>();
            CarregarSensoresCommand = new RelayCommand(CarregarSensores);
            SalvarConfiguracaoCommand = new RelayCommand(SalvarConfiguracao);

            // Carrega o limite atual do banco assim que a tela abre
            CarregarConfiguracao();
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
                        ListaSensores.Add(registro);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao carregar dados: {ex.Message}");
            }
        }

        // =========================================================
        // NOVOS MÉTODOS DO DESAFIO DE CONFIGURAÇÃO
        // =========================================================

        private async void CarregarConfiguracao()
        {
            try
            {
                var http = new HttpClient();
                var config = await http.GetFromJsonAsync<ConfiguracaoDto>("https://localhost:44379/api/v1/sensores/configuracao");
                if (config != null)
                {
                    TemperaturaMaxima = config.TemperaturaMaxima; // Preenche a caixinha de texto da tela
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao carregar configuração: {ex.Message}");
            }
        }

        private async void SalvarConfiguracao()
        {
            try
            {
                var http = new HttpClient();
                // Envia o valor da propriedade TemperaturaMaxima para a nossa API salvar no SQLite
                var response = await http.PostAsJsonAsync("https://localhost:44379/api/v1/sensores/configuracao", TemperaturaMaxima);

                if (response.IsSuccessStatusCode)
                {
                    MessageBox.Show("Configuração salva com sucesso no Banco de Dados!\n\nO simulador já vai usar o novo limite.", "Desafio Concluído", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                else
                {
                    MessageBox.Show("Erro ao salvar configuração na API.", "Erro", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro de conexão: {ex.Message}", "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }

    // Classe auxiliar para ler o JSON da API (igual fizemos no simulador)
    public class ConfiguracaoDto
    {
        public double TemperaturaMaxima { get; set; }
    }
}
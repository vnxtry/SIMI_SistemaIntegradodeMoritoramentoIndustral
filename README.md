# 🏭 SIMI - Sistema Integrado de Monitoramento Industrial

Este repositório contém a solução completa para a Situação de Aprendizagem (SA) de monitoramento de sinais industriais. O sistema utiliza uma arquitetura distribuída para simular, processar, armazenar e visualizar dados de telemetria em tempo real.

## 🚀 Requisitos Implementados

Conforme solicitado nos requisitos da avaliação, o projeto entrega:

1.  **Novo Sensor Industrial**: Implementação do sinal de **Pressão (PSI)**, além da Temperatura.
2.  **Persistência Local**: Integração com **SQLite** via Entity Framework Core para armazenamento de dados histórico.
3.  **Documentação Profissional**: API totalmente documentada com **Swagger (OpenAPI)**, utilizando comentários XML para descrição de endpoints e modelos.
4.  **Interface de Visualização**: Aplicação **WPF** que consome a API para exibir os logs do banco de dados ao operador.

---

## 🛠️ Arquitetura da Solução

O projeto está dividido em quatro camadas principais:

* **`Shared`**: Biblioteca de classes contendo o modelo de dados `SensorData`, garantindo que todos os projetos utilizem o mesmo contrato.
* **`ApiProcessamento`**: O núcleo do sistema. Recebe os dados, aplica regras de negócio (limite de temperatura) e persiste as informações no SQLite.
* **`SensorSimulator`**: Aplicação de consola que emula um hardware industrial, enviando pacotes JSON com Temperatura e Pressão para a API.
* **`SensorInterface`**: Interface gráfica (WPF) que permite ao utilizador visualizar o histórico de dados armazenados no banco.

---

## 📋 Documentação da API (Endpoints)

A API foi configurada para gerar documentação automática via Swagger, acessível pela rota `/swagger`.

### Endpoints Principais:

* **`POST /api/v1/sensores`**
    * **Função**: Recebe telemetria do simulador.
    * **Regra**: Se a temperatura ultrapassar o limite configurado no `ApiConfig`, a API rejeita o dado com erro `400 Bad Request`.
    * **Persistência**: Grava o ID, Temperatura, Pressão e Timestamp no ficheiro `sensores.db`.
    
* **`GET /api/v1/sensores`**
    * **Função**: Retorna a lista completa de leituras armazenadas.
    * **Uso**: Utilizado pela interface WPF para popular o histórico de monitoramento.

---

## 🔧 Como Executar

### 1. Preparação do Banco de Dados
Para criar o banco de dados SQLite localmente, execute o seguinte comando no **Consola do Gestor de Pacotes** (selecionando `ApiProcessamento` como projeto padrão):
```powershell
Update-Database

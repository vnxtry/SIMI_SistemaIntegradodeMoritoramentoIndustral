# 🏭 SIMI - Sistema Integrado de Monitoramento Industrial

Este projeto consiste em uma solução de monitoramento de telemetria industrial desenvolvida em .NET 8. O sistema simula a coleta de dados de sensores, processa essas informações via API, armazena em banco de dados local e exibe os resultados em uma interface gráfica.

## 🚀 Requisitos Cumpridos (SA)

- **Novo Sensor Implementado:** Além da temperatura, o sistema agora monitora a **Pressão (PSI)**.
- **Persistência em Banco de Dados:** Implementação de **SQLite** via Entity Framework Core para armazenamento permanente.
- **Documentação Técnica:** API documentada utilizando **Swagger UI** com comentários XML detalhados.
- **Padrão Arquitetural:** Separação de responsabilidades em 4 projetos (Shared, Api, Simulator e Interface).

---

## 🛠️ Estrutura do Projeto

1.  **Shared**: Biblioteca de classes contendo o modelo `SensorData` (Contrato comum).
2.  **ApiProcessamento**: Web API responsável pela lógica de negócio, validação de limites térmicos e persistência.
3.  **SensorSimulator**: Aplicativo Console que simula o hardware industrial gerando dados aleatórios de temperatura e pressão.
4.  **SensorInterface**: Aplicativo Desktop (WPF) que consome a API para exibição dos dados ao usuário final.

---

## 📋 Documentação da API (Endpoints)

A API utiliza o Swagger para documentação interativa. Os principais endpoints são:

* **`POST /api/v1/sensores`**
    * **Descrição**: Recebe e valida os dados de telemetria.
    * **Regra de Negócio**: Se a temperatura exceder o limite definido no arquivo de configuração, a API retorna `400 Bad Request`.
    * **Persistência**: Dados válidos são gravados automaticamente no arquivo `sensores.db`.
* **`GET /api/v1/sensores`**
    * **Descrição**: Recupera o histórico completo de leituras do banco de dados para alimentar a interface WPF.

---

## 🔧 Como Executar o Projeto

Siga estes passos para rodar a aplicação localmente:

1.  **Preparar o Banco de Dados**:
    * Abra o **Console do Gerenciador de Pacotes** no Visual Studio.
    * Selecione o projeto `ApiProcessamento` como projeto padrão.
    * Execute o comando:  
        `Update-Database`
2.  **Iniciar a Solução**:
    * Clique com o botão direito na **Solução** > **Configurar Projetos de Inicialização**.
    * Selecione **Vários projetos de inicialização**.
    * Defina como "Iniciar": `ApiProcessamento`, `SensorSimulator` e `SensorInterface`.
3.  **Acessar a Documentação**:
    * Ao rodar a API, o navegador abrirá automaticamente em `/swagger/index.html`.

---

## 📦 Tecnologias Utilizadas
- .NET 8 (C#)
- Entity Framework Core (EF Core)
- SQLite
- Swagger / OpenAPI
- WPF (Windows Presentation Foundation)

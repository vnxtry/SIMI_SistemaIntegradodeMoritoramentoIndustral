# SIMI - Sistema Integrado de Monitoramento Industrial

Projeto desenvolvido para a Situação de Aprendizagem de monitoramento de sensores.

## 🚀 Endpoints da API
- **GET /api/v1/sensores**: Retorna o histórico de todas as leituras de Temperatura e Pressão salvas no banco SQLite.
- **POST /api/v1/sensores**: Recebe telemetria em tempo real. Se a temperatura exceder o limite (ApiConfig), retorna erro 400.

## 🛠️ Como Executar
1. Execute `Update-Database` no Console do Gerenciador de Pacotes para criar o banco.
2. Inicie a API para habilitar o Swagger.
3. Inicie o Simulador e a Interface.

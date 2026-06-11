# FinControl 💰

Sistema de gestão financeira pessoal desenvolvido em C# para controle de receitas, despesas e análise financeira.

## 📌 Sobre o Projeto

O FinControl foi desenvolvido como projeto de estudos para praticar conceitos fundamentais de desenvolvimento de software utilizando C# e .NET.

A aplicação permite registrar movimentações financeiras, acompanhar o saldo, organizar despesas por categorias e gerar relatórios detalhados para diferentes períodos.

## 🚀 Funcionalidades

### Gerenciamento de Transações

* Cadastro de receitas e despesas
* Edição de transações
* Exclusão de transações
* Busca de transações por descrição
* Categorias pré-definidas
* Registro automático de data e hora
* Identificador único para cada transação

### Controle Financeiro

* Visualização do saldo atual
* Cálculo automático de receitas
* Cálculo automático de despesas

### Relatórios Financeiros

* Relatório Geral
* Relatório Mensal
* Relatório Anual
* Relatório Personalizado por período
* Análise de gastos por categoria
* Ordenação das transações por data

### Persistência de Dados

* Armazenamento em arquivo JSON
* Carregamento automático dos dados ao iniciar o sistema
* Salvamento automático após alterações

## 🛠️ Tecnologias Utilizadas

* C#
* .NET
* LINQ
* System.Text.Json
* Programação Orientada a Objetos (POO)

## 📂 Estrutura do Projeto

```text
FinControl
├── Models
│   └── Transacao.cs
├── Services
│   └── RelatorioService.cs
├── Data
│   └── transacoes.json
└── Program.cs
```

## 📊 Exemplo de Relatório

```text
====================================
RELATÓRIO DE JUNHO DE 2026
====================================

Total de Receitas: R$ 3.000,00
Total de Despesas: R$ 550,00
Saldo Final: R$ 2.450,00

=== GASTOS POR CATEGORIA ===

Alimentação: R$ 500,00
Transporte: R$ 50,00
```

## 📚 Conceitos Praticados

Durante o desenvolvimento deste projeto foram aplicados conceitos como:

* Estruturas de repetição
* Estruturas condicionais
* Métodos
* Listas e coleções
* Manipulação de arquivos
* Serialização e desserialização JSON
* LINQ
* Organização em camadas
* Validação de entrada de dados
* Boas práticas de programação

## 🔮 Próximas Melhorias

* Dashboard financeiro inicial
* Exportação para CSV
* Banco de dados SQLite
* Gráficos financeiros
* Interface gráfica
* Testes automatizados

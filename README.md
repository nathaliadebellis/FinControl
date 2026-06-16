# 💰 FinControl

FinControl é uma aplicação de console desenvolvida em C# e .NET para gerenciamento de finanças pessoais. O sistema permite registrar receitas e despesas, categorizar movimentações, acompanhar o saldo financeiro, visualizar um dashboard com indicadores financeiros, gerar relatórios detalhados e manter os dados protegidos por meio de um sistema de backup e recuperação.

Projeto desenvolvido com foco no aprendizado de Programação Orientada a Objetos (POO), LINQ, serialização JSON, tratamento de exceções, validação de dados e boas práticas de desenvolvimento com .NET.

---

## 📌 Sobre o Projeto

O FinControl foi criado para simular um sistema de controle financeiro capaz de auxiliar no gerenciamento de receitas e despesas de forma simples e organizada.

Além das operações básicas de cadastro, edição e exclusão de transações, o projeto implementa recursos de confiabilidade como backup automático, restauração de dados, tratamento centralizado de erros e persistência em arquivos JSON.

---

## 🚀 Funcionalidades

### 📝 Gerenciamento de Transações

* Cadastro de receitas e despesas
* Edição de transações existentes
* Exclusão de transações
* Busca de transações por descrição
* Categorização das movimentações financeiras
* Registro automático de data e hora
* Geração de identificadores únicos para cada transação

### 💵 Controle Financeiro

* Exibição do saldo atual
* Cálculo automático de receitas e despesas
* Atualização dinâmica do saldo com base nas movimentações cadastradas

### 📈 Dashboard Financeiro

- Visualização consolidada do saldo atual
- Exibição do total de receitas e despesas
- Identificação da categoria com maior gasto
- Destaque para a maior receita e maior despesa registradas
- Resumo das últimas transações
- Distribuição percentual dos gastos por categoria

### 📊 Relatórios 

* Relatório financeiro geral
* Relatório mensal
* Relatório anual
* Relatório personalizado por período
* Análise de gastos por categoria
* Ordenação das transações por data

### 💾 Persistência de Dados

* Armazenamento em arquivo JSON
* Carregamento automático dos dados ao iniciar a aplicação
* Salvamento automático após alterações

### 🛡️ Backup e Recuperação

* Criação automática de backup antes de salvar os dados
* Criação manual de backups
* Listagem dos backups disponíveis
* Restauração completa dos dados a partir de backups
* Limpeza automática de backups antigos
* Exibição de informações sobre os backups armazenados

### ⚠️ Tratamento de Erros

* Gerenciamento centralizado de exceções
* Registro de erros em log
* Consulta aos erros recentes
* Validação das entradas do usuário


---

## 🛠️ Tecnologias Utilizadas

* C#
* .NET
* LINQ
* System.Text.Json
* Programação Orientada a Objetos (POO)
* Manipulação de Arquivos
* Tratamento de Exceções

---

## 📂 Estrutura do Projeto

```text
FinControl/
├── Data/
│   └── transacoes.json
│
├── Models/
│   ├── Categorias.cs
│   ├── FiltroTransacao.cs
│   ├── TipoTransacao.cs
│   └── Transacao.cs
│
├── Services/
│   ├── BuscaTransacaoService.cs
│   ├── DashboardServices.cs
│   ├── ExcecoesCustomizadas.cs
│   ├── Formatting.cs
│   ├── GerenciadorErros.cs
│   ├── LoggerArquivos.cs
│   ├── RelatorioService.cs
│   └── ValidadorEntrada.cs
│
└── Program.cs
```


---

## ▶️ Como Executar

1. Clone o repositório:

```bash
git clone https://github.com/nathaliadebellis/FinControl.git
```

2. Acesse a pasta do projeto:

```bash
cd FinControl
```

3. Execute a aplicação:

```bash
dotnet run
```

---

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

---

## 📚 Conceitos Aplicados

Durante o desenvolvimento deste projeto foram aplicados conceitos como:

* Programação Orientada a Objetos (POO)
* Estruturas condicionais e de repetição
* Métodos e modularização
* Organização em camadas
* Manipulação de listas e coleções
* Consultas utilizando LINQ
* Serialização e desserialização JSON
* Manipulação de arquivos
* Tratamento de exceções
* Logging de erros
* Validação de entrada de dados
* Persistência de informações
* Backup e recuperação de dados
* Agregação e análise de dados com LINQ para construção de dashboards

---

## 🔮 Próximas Melhorias

- Exportação de relatórios para CSV
- Persistência utilizando SQLite
- Interface gráfica (WPF, WinForms ou Blazor)
- Testes automatizados
- Injeção de Dependência (Dependency Injection)
- Arquitetura baseada em interfaces

---

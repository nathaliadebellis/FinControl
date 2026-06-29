# 💰 FinControl

![C#](https://img.shields.io/badge/C%23-12-68217A?logo=csharp&logoColor=white)
![.NET](https://img.shields.io/badge/.NET-9-512BD4?logo=dotnet&logoColor=white)
![Repository Pattern](https://img.shields.io/badge/Repository-Pattern-0A66C2)
![Dependency Injection](https://img.shields.io/badge/Dependency-Injection-8A2BE2)
![Architecture](https://img.shields.io/badge/Architecture-Layered-009688)
![JSON](https://img.shields.io/badge/Persistence-JSON-FF9800)
![License](https://img.shields.io/badge/License-MIT-4CAF50)
![Status](https://img.shields.io/badge/Status-Active-success)

> Sistema de gerenciamento financeiro pessoal desenvolvido em **C#** e **.NET**, com arquitetura em camadas, persistência em JSON, dashboard financeiro, planejamento de orçamento e aplicação de boas práticas de desenvolvimento.

---

## ✨ Visão Geral

O **FinControl** é uma aplicação de console desenvolvida para auxiliar no gerenciamento de finanças pessoais.

O projeto permite registrar receitas e despesas, categorizar movimentações financeiras, acompanhar indicadores por meio de um dashboard, definir metas de economia, controlar orçamentos por categoria e gerar relatórios financeiros.

Além das funcionalidades de negócio, o projeto foi desenvolvido com foco na aplicação prática de conceitos de engenharia de software, arquitetura em camadas e boas práticas de desenvolvimento utilizando **C#** e **.NET**.

---

# 🚀 Funcionalidades

## 📝 Gerenciamento de Transações

- Cadastro de receitas e despesas
- Edição de transações
- Exclusão de transações
- Busca por descrição
- Categorização das movimentações
- Registro automático de data e hora
- Geração automática de identificadores únicos

---

## 💵 Controle Financeiro

- Exibição do saldo atual
- Cálculo automático de receitas e despesas
- Atualização dinâmica do saldo financeiro

---

## 📈 Dashboard Financeiro

- Saldo consolidado
- Total de receitas
- Total de despesas
- Percentual de economia
- Índice de Saúde Financeira
- Categoria com maior gasto
- Maior receita e maior despesa
- Últimas transações cadastradas
- Distribuição percentual dos gastos por categoria
- Análises automáticas para apoio à tomada de decisão

---

## 🎯 Planejamento Financeiro

- Cadastro de orçamentos por categoria
- Definição de limites mensais
- Alertas automáticos de orçamento
- Definição e acompanhamento de metas de economia

---

## 📊 Relatórios

- Relatório Geral
- Relatório Mensal
- Relatório Anual
- Relatório por período
- Gastos por categoria

---

## 💾 Persistência

- Persistência em arquivos JSON
- Carregamento automático dos dados
- Salvamento automático após alterações

---

## 🛡️ Backup e Recuperação

- Backup automático
- Backup manual
- Restauração de backups
- Listagem dos backups disponíveis
- Limpeza automática de backups antigos
- Informações sobre backups armazenados

---

## ⚠️ Tratamento de Erros

- Tratamento centralizado de exceções
- Registro de erros em log
- Consulta aos erros recentes
- Validação das entradas do usuário

---

# 🏗️ Arquitetura

O projeto segue uma arquitetura em camadas, promovendo baixo acoplamento, alta coesão e separação de responsabilidades.

```text
                Usuário
                    │
                    ▼
             Menus (Console UI)
                    │
                    ▼
                Services
         (Regras de Negócio)
                    │
                    ▼
             Repositories
          (Persistência JSON)
                    │
                    ▼
             Arquivos JSON
```

## Estrutura das Camadas

- **Menus:** interação com o usuário.
- **Services:** regras de negócio.
- **Repositories:** persistência dos dados.
- **Interfaces:** abstração dos repositórios.
- **Models:** entidades da aplicação.
- **Utils:** componentes auxiliares.

---

# 🛠️ Tecnologias e Conceitos Aplicados

### Linguagem e Plataforma

- C#
- .NET

### Arquitetura e Engenharia de Software

- Programação Orientada a Objetos (POO)
- Organização em Camadas
- Repository Pattern
- Injeção de Dependência (Dependency Injection)
- Interfaces
- Separação de Responsabilidades (Separation of Concerns)

### Desenvolvimento

- LINQ
- Métodos e Modularização
- Manipulação de Listas e Coleções
- Estruturas Condicionais e de Repetição

### Persistência de Dados

- Manipulação de Arquivos
- Serialização e Desserialização JSON
- Persistência de Dados

### Qualidade e Confiabilidade

- Tratamento de Exceções
- Logging de Erros
- Backup e Recuperação de Dados
- Validação de Entradas

### Regras de Negócio

- Planejamento Financeiro
- Controle de Orçamentos
- Metas de Economia
- Dashboard Financeiro
- Indicadores Financeiros
- Agregação e análise de dados utilizando LINQ

---

# 📂 Estrutura do Projeto

```text
FinControl/
│
├── Data/
│   ├── transacoes.json
│   ├── orcamentos.json
│   ├── metaEconomia.json
│   └── backup/
│
├── Interfaces/
│   ├── IMetaEconomiaRepository.cs
│   ├── IOrcamentoRepository.cs
│   └── ITransacaoRepository.cs
│
├── Menus/
│   ├── MenuPlanejamentoFinanceiro.cs
│   ├── MenuRelatorios.cs
│   ├── MenuSistema.cs
│   ├── MenuTransacoes.cs
│   └── MenuVisaoGeral.cs
│
├── Models/
│   ├── Categorias.cs
│   ├── DashboardResumo.cs
│   ├── FiltroTransacao.cs
│   ├── GastoCategoriaResumo.cs
│   ├── MetaEconomia.cs
│   ├── OrcamentoCategoria.cs
│   ├── TipoTransacao.cs
│   └── Transacao.cs
│
├── Repositories/
│   ├── JsonMetaEconomiaRepository.cs
│   ├── JsonOrcamentoRepository.cs
│   └── JsonTransacaoRepository.cs
│
├── Services/
│   ├── DashboardService.cs
│   ├── FinanceiroService.cs
│   ├── MetaEconomiaService.cs
│   ├── OrcamentoService.cs
│   ├── RelatorioService.cs
│   ├── SistemaService.cs
│   └── TransacaoService.cs
│
├── Utils/
│   ├── ExcecoesCustomizadas.cs
│   ├── Formatting.cs
│   ├── GerenciadorErros.cs
│   ├── LoggerArquivos.cs
│   └── ValidadorEntrada.cs
│
└── Program.cs
```

---

# ▶️ Como Executar

### Clone o repositório

```bash
git clone https://github.com/nathaliadebellis/FinControl.git
```

### Acesse a pasta

```bash
cd FinControl
```

### Execute o projeto

```bash
dotnet run
```

---

# 📊 Exemplo de Relatório

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

# 📌 Roadmap

- [x] Cadastro de receitas e despesas
- [x] Dashboard financeiro
- [x] Planejamento financeiro
- [x] Metas de economia
- [x] Relatórios financeiros
- [x] Persistência em JSON
- [x] Backup automático
- [x] Repository Pattern
- [x] Injeção de Dependência
- [x] Arquitetura em Camadas
- [ ] Persistência utilizando SQLite ou SQL Server
- [ ] Entity Framework Core
- [ ] Exportação para CSV, Excel e PDF
- [ ] Testes unitários com xUnit e Moq
- [ ] Registro de dependências utilizando Microsoft.Extensions.DependencyInjection
- [ ] Interface gráfica (WPF, .NET MAUI ou Blazor)
- [ ] Sistema de autenticação
- [ ] Múltiplos usuários
- [ ] Gráficos financeiros
- [ ] Configurações do usuário
- [ ] Notificações inteligentes

---

# 🎯 Objetivo

Este projeto faz parte da minha transição de carreira para a área de desenvolvimento de software e tem como objetivo demonstrar a aplicação prática de conceitos de **C#**, **.NET**, **Programação Orientada a Objetos**, **arquitetura em camadas**, **injeção de dependência**, **Repository Pattern** e boas práticas de engenharia de software.

O FinControl continuará evoluindo com novas funcionalidades, integrações e melhorias arquiteturais, servindo como um projeto de estudo, portfólio e demonstração prática das tecnologias utilizadas no ecossistema .NET.

// using FinControl.Models;
// using Microsoft.EntityFrameworkCore;
// 
// public class FinControlContext : DbContext
// {
// public DbSet<Transacao> Transacoes => Set<Transacao>();
// 
// public DbSet<OrcamentoCategoria> Orcamentos => Set<OrcamentoCategoria>();
// 
// public DbSet<MetaEconomia> Metas => Set<MetaEconomia>();
// 
// protected override void OnConfiguring(
// DbContextOptionsBuilder optionsBuilder)
// {
// optionsBuilder.UseSqlServer(
// @"Server=.\SQLEXPRESS;
// Database = FinControl;
// Trusted_Connection = True;
// TrustServerCertificate = True");
//     }
// }
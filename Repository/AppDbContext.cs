using DinamicaXP.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace DinamicaXP.Repository
{
    public class AppDbContext : DbContext
    {
        public DbSet<Cliente> Clientes { get; set; }
        public DbSet<Pagamento> Pagamentos { get; set; }
        public DbSet<Conta> Contas { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // Definindo a string de conexão para o MySQL
            var connectionString = "server=localhost;database=DinamicaXP;user=root;password=root";

            // Configurando o provedor MySQL para a string de conexão
            optionsBuilder.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configuração da entidade 'Cliente'
            modelBuilder.Entity<Cliente>(entity =>
            {
                entity.ToTable("clientes");

                // Chave primária
                entity.HasKey(c => c.Id);
                entity.Property(c => c.Id)
                      .HasColumnName("id");

                // Coluna 'name' (nome do cliente)
                entity.Property(c => c.Name)
                      .HasColumnName("name")
                      .IsRequired();

                // Relacionamento 1:N com Pagamentos
                entity.HasMany(c => c.Pagamentos)
                      .WithOne(p => p.Cliente)
                      .HasForeignKey(p => p.ClienteId)
                      .OnDelete(DeleteBehavior.SetNull); // ou SetNull se desejar manter os pagamentos mesmo se o cliente for removido

                entity.HasOne(c => c.Conta)
                      .WithOne(c => c.Cliente)
                      .HasForeignKey<Cliente>(c => c.ContaId)  // Chave estrangeira na entidade Cliente
                      .IsRequired(false)  // Relacionamento opcional
                      .OnDelete(DeleteBehavior.SetNull);  // Quando deletar o cliente, a FK é setada como null
            });

            // Configuração da entidade 'Conta'
            modelBuilder.Entity<Conta>(entity =>
            {
                entity.ToTable("contas");

                // Chave primária
                entity.HasKey(c => c.Id);
                entity.Property(c => c.Id)
                        .HasColumnName("id");

                // Coluna 'valor_pago' e 'saldo_devido' com tipo decimal
                entity.Property(c => c.ValorPago)
                        .HasColumnName("valor_pago")
                        .HasColumnType("decimal(18,2)");

                entity.Property(c => c.SaldoDevido)
                        .HasColumnName("saldo_devido")
                        .HasColumnType("decimal(18,2)");

                entity.HasOne(c => c.Cliente)
                        .WithOne(c => c.Conta)
                        .HasForeignKey<Conta>(c => c.ClienteId)  // Chave estrangeira na entidade Conta
                        .IsRequired(false)  // Relacionamento opcional
                        .OnDelete(DeleteBehavior.SetNull);  // Quando deletar a conta, a FK é setada como null

            });

            // Configuração da entidade 'Pagamento'
            modelBuilder.Entity<Pagamento>(entity =>
            {
                entity.ToTable("pagamentos");

                // Chave primária
                entity.HasKey(p => p.Id);
                entity.Property(p => p.Id)
                      .HasColumnName("id");

                // Coluna 'value' com tipo decimal
                entity.Property(p => p.Value)
                      .HasColumnName("value")
                      .HasColumnType("decimal(18,2)");

                // Coluna 'pago' (indica se o pagamento foi realizado)
                entity.Property(p => p.Pago)
                      .HasColumnName("pago");

                // Coluna 'data' (data do pagamento)
                entity.Property(p => p.Data)
                      .HasColumnName("data");

                // Configuração da chave estrangeira 'cliente_id' (opcional)
                entity.HasOne(p => p.Cliente)
                      .WithMany(cl => cl.Pagamentos)  // Um cliente pode ter vários pagamentos
                      .HasForeignKey(p => p.ClienteId)
                      .IsRequired(false)  // Relacionamento opcional
                      .OnDelete(DeleteBehavior.SetNull);  // Quando um cliente for excluído, os pagamentos não são excluídos, mas o ClienteId será NULL
            });


        }

    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DinamicaXP.Models
{
    [Table("contas")]
    public class Conta
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Column("valor_pago", TypeName = "decimal(18,2)")]
        public decimal ValorPago { get; set; } = 0M;

        [Column("saldo_devido", TypeName = "decimal(18,2)")]
        public decimal SaldoDevido { get; set; } = 0M;

        [ForeignKey("Cliente")]
        [Column("cliente_id")]
        public int? ClienteId { get; set; }

        public virtual Cliente Cliente { get; set; }
    }
}

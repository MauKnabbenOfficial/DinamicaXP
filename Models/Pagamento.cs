using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DinamicaXP.Models
{
    [Table("pagamentos")]
    public class Pagamento
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Column("value", TypeName = "decimal(18,2)")]
        public decimal Value { get; set; }

        [Column("pago")]
        public bool Pago { get; set; }

        [Column("data")]
        public DateTime Data { get; set; }

        [ForeignKey("Cliente")]
        [Column("cliente_id")]
        public int? ClienteId { get; set; }

        public virtual Cliente Cliente { get; set; }
    }
}

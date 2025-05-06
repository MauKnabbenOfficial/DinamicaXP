using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DinamicaXP.Models
{
    [Table("clientes")]
    public class Cliente
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Column("id")]
        public int Id { get; set; }

        [Required]
        [Column("name")]
        public string Name { get; set; }

        [ForeignKey("Conta")]
        [Column("conta_id")]
        public int? ContaId { get; set; }

        public virtual Conta Conta { get; set; }
        public virtual ICollection<Pagamento> Pagamentos { get; set; }
    }
}




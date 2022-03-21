using System.ComponentModel.DataAnnotations.Schema;

namespace atm.Models
{
    public class Transaction : BaseModel
    {
        public int AtmId { get; set; }
        [ForeignKey("AtmId")] public ATM Atm { get; set; }

        public int AccountId { get; set; }
        [ForeignKey("AccountId")] public Account Account { get; set; }
        
        [Column(TypeName = "decimal(18,2)")]
        public decimal Amount { get; set; }

        public string CurrentAllocation { get; set; }
        public string BestAllocation { get; set; }
    }
}
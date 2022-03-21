using System.ComponentModel.DataAnnotations.Schema;

namespace atm.Models
{
    public class AccountHistory : BaseModel
    {
        [Column(TypeName = "decimal(18,4)")] public decimal Amount { get; set; }

        public int AccountId { get; set; }
        [ForeignKey("AccountId")] public Account Account { get; set; }
    }
}
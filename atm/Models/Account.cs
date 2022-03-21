using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace atm.Models
{
    public class Account : BaseModel
    {
        [Column(TypeName = "decimal(18,4)")] public decimal Balance { get; set; } = decimal.Zero;

        public int UserId { get; set; }
        [ForeignKey("UserId")] public User User { get; set; }

        public ICollection<AccountHistory> Histories { get; set; } = new List<AccountHistory>();
    }
}
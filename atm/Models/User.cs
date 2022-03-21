using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace atm.Models
{
    public class User : BaseModel
    {
        [Column(TypeName = "varchar(100)")] public string Name { get; set; }

        public int RoleId { get; set; }
        [ForeignKey("RoleId")] public Role Role { get; set; }

        public ICollection<Account> Accounts { get; set; }
    }
}
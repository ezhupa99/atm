using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace atm.Models
{
    public class Role : BaseModel
    {
        [Column(TypeName = "varchar(50)")]
        public string Name { get; set; }
    }
}
using System;

namespace atm.Models
{
    public interface IBaseModel
    {
        public int Id { get; set; }

        public DateTime Created { get; set; }

        public DateTime? Updated { get; set; }

        public DateTime? Deleted { get; set; }
    }
}
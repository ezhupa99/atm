namespace atm.Models
{
    public class ATM : BaseModel
    {
        public int Average { get; set; } = 0;
        public int Banknote5000 { get; set; } = 0;
        public int Banknote2000 { get; set; } = 0;
        public int Banknote1000 { get; set; } = 0;
        public int Banknote500 { get; set; } = 0;
    }
}
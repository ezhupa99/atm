namespace atm.Models
{
    public class ATM : BaseModel
    {
        public int Average { get; set; }
        public int Banknote5000 { get; set; }
        public int Banknote2000 { get; set; }
        public int Banknote1000 { get; set; }
        public int Banknote500 { get; set; }
    }
}
namespace atm.Models.ViewModels
{
    public class AtmDto
    {
        public int Id { get; set; }
        public int Banknote5000 { get; set; } = 0;
        public int Banknote2000 { get; set; } = 0;
        public int Banknote1000 { get; set; } = 0;
        public int Banknote500 { get; set; } = 0;
    }
}
namespace atm.Models.ViewModels
{
    public class AccountDto
    {
        public int Id { get; set; }
        public decimal Balance { get; set; } = decimal.Zero;
    }
}
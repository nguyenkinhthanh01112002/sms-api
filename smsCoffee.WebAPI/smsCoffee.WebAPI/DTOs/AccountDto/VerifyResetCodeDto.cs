namespace smsCoffee.WebAPI.DTOs.AccountDto
{
    public class VerifyResetCodeDto  
    {
        public string EmailOrPhone { get; set; }
        public string Otp { get; set; }
    }
}

namespace smsCoffee.WebAPI.DTOs.AccountDto
{
    public class NewUserDto
    {
        public string UserName { get; set; }
        public string Token { get; set; }
        public string RefreshToken {  get; set; }
    }
}

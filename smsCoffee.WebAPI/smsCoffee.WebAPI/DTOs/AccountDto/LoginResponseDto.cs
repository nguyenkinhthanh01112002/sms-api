namespace smsCoffee.WebAPI.DTOs.AccountDto
{
    public class LoginResponseDto
    {
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
        public int ExpireTime { get; set; }
    }
}

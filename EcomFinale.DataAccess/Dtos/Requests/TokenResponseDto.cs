namespace EcomFinale.DataAccess.Dtos.Requests;

public class TokenResponseDto
{
    public string AccessToken { get; set; }
    public string RefreshToken { get; set; }
}
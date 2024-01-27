namespace Workhub.Contracts.Authentication;

public record SellerLoginRequest(string Email,
    string Password)
{
}

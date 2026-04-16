namespace Workhub.Application.Interfaces.Persistance
{
    public interface ICheckVerify
    {
        Task<bool> checkVerifyStats(string email);
        Task<bool> ConfirmEmail(string token, string email);
        Task<bool> ResendOTP(string email);
    }
}
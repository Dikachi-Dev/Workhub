using FirebaseAdmin;
using Google.Apis.Auth.OAuth2;

namespace Workhub.Api.Configurations
{
    public static class AuthConfigurations
    {
        public static void AddAuthConfigurations(this IServiceCollection services)
        {
            GoogleCredential credential = GoogleCredential.FromFile("firebaseset.json");
            try
            {
                FirebaseApp.Create(new AppOptions()
                {
                    Credential = credential
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}

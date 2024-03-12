using ErrorOr;

namespace Workhub.Domain.Errors;
public static partial class Errors
{
    public static partial class Authentication
    {
        public static Error InvalidCredentials => Error.Conflict(
            code: "Authentication.InvalidCredentials",
            description: "Invalid user name or password");

        public static Error NotVerified = Error.Conflict(
            code:"Authentication.NotVerified",
            description: "Not Verified"
        );


    }
}

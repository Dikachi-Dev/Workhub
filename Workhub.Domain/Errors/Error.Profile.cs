using ErrorOr;

namespace Workhub.Domain.Errors;
public static partial class Errors
{
    public static partial class Profile
    {
        public static Error DuplicateEmail => Error.Conflict(
            code: "Profile.DuplicateEmail",
            description: "Email already exists");

        public static Error NotFound => Error.NotFound(
            code: "Profile.NotFound",
            description: " NotFound");
    }
}



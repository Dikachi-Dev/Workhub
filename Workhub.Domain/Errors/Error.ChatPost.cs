using ErrorOr;

namespace Workhub.Domain.Errors;

public static partial class Errors
{
    public static partial class ChatPost
    {
        public static Error NotFound => Error.NotFound(
            description: "Chat searched does not exist",
            code: "ChatPost.NotFound"
            );
    }
}

using ErrorOr;

namespace Workhub.Domain.Errors;

public static partial class Errors
{
    public static partial class Job
    {
        public static Error NotFound => Error.NotFound(
            description: "Job searched does not exist",
            code: "Job.NotFound"
            );
    }
}

using Workhub.Domain.Entities;

namespace Workhub.Application.Profiless.Common;

public record GetAllResult(IEnumerable<MyProfileResult> Profiles);

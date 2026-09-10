using ErrorOr;
using MediatR;
using NetTopologySuite.Geometries;
using Workhub.Application.Interfaces.Persistance;
using Workhub.Application.Interfaces.Services;

namespace Workhub.Application.Profiless.Commands;

public record UpdateLocationCommand(
    string UserId,
    string LongLat) : IRequest<ErrorOr<Success>>;

public class UpdateLocationCommandHandler : IRequestHandler<UpdateLocationCommand, ErrorOr<Success>>
{
    private readonly IProfileRepository repository;
    private readonly ICloseProx closeProx;

    public UpdateLocationCommandHandler(IProfileRepository repository, ICloseProx closeProx)
    {
        this.repository = repository;
        this.closeProx = closeProx;
    }

    public async Task<ErrorOr<Success>> Handle(UpdateLocationCommand request, CancellationToken cancellationToken)
    {
        var profile = await repository.GetById(request.UserId);
        if (profile is null)
        {
            return Domain.Errors.Errors.Profile.NotFound;
        }

        var response = await closeProx.GetFullAddress(request.LongLat);
        if (response == null)
        {
            return Error.Failure(description: "Failed to resolve address for coordinates.");
        }

        // Parse PostGIS Location Point (longitude, latitude)
        Point? point = null;
        var parts = request.LongLat.Split(',');
        if (parts.Length >= 2 &&
            double.TryParse(parts[0].Trim(), out double lat) &&
            double.TryParse(parts[1].Trim(), out double lon))
        {
            point = new Point(lon, lat) { SRID = 4326 };
        }

        if (point == null)
        {
            return Error.Validation(description: "Invalid coordinates format.");
        }

        profile.UpdateLocation(point, request.LongLat, (string)response.Country, (string)response.State, (string)response.Address);

        repository.Update(profile);
        await repository.SaveChanges();

        return Result.Success;
    }
}

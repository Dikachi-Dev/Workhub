using NetTopologySuite.Geometries;

namespace Workhub.Infrastructure.Helpers;

public static class GeospatialHelper
{
    private static readonly GeometryFactory _geometryFactory = 
        new GeometryFactory(new PrecisionModel(), 4326);
    
    /// <summary>
    /// Parse "latitude,longitude" string to PostGIS Point
    /// </summary>
    public static Point? ParseLongLat(string? longLat)
    {
        if (string.IsNullOrWhiteSpace(longLat)) return null;
        
        var parts = longLat.Split(',');
        if (parts.Length != 2) return null;
        
        if (double.TryParse(parts[0].Trim(), out var lat) && 
            double.TryParse(parts[1].Trim(), out var lon))
        {
            // Note: PostGIS Point is (longitude, latitude)
            return _geometryFactory.CreatePoint(new Coordinate(lon, lat));
        }
        
        return null;
    }
    
    /// <summary>
    /// Create PostGIS Point from latitude and longitude
    /// </summary>
    public static Point CreatePoint(double latitude, double longitude)
    {
        return _geometryFactory.CreatePoint(new Coordinate(longitude, latitude));
    }
    
    /// <summary>
    /// Convert Point to "latitude,longitude" string
    /// </summary>
    public static string? PointToLongLat(Point? point)
    {
        if (point == null) return null;
        return $"{point.Y},{point.X}"; // Y is latitude, X is longitude
    }
}

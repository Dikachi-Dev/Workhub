using Newtonsoft.Json.Linq;
using Workhub.Application.Interfaces.Services;
using Workhub.Domain.Entities;

namespace Workhub.Infrastructure.Services;
public class CloseProx : ICloseProx
{
    public async Task<List<Profile>> GetProfilesSortedByProximity(string origin, string destinations, IEnumerable<Profile> profiles)
    {
        List<Profile> closeProximity = new List<Profile>(); // Initialize list
        string apiKey = "AIzaSyAQyKyxbcnwmRFW1OyJHuhKhCuaXeU3bmg";
        var httpcon = new HttpClient();
        string url = $"https://maps.googleapis.com/maps/api/distancematrix/json?origins={origin}&destinations={destinations}&key={apiKey}";
        HttpResponseMessage response = await httpcon.GetAsync(url);

        if (response.IsSuccessStatusCode)
        {
            string details = await response.Content.ReadAsStringAsync();
            JObject json = JObject.Parse(details);
            var distanceElements = json["rows"][0]["elements"].Children();

            // Create a list to store distances and corresponding indices
            List<(int distance, int index)> distances = new List<(int distance, int index)>();

            int index = 0;

            foreach (var distanceElement in distanceElements)
            {
                var distanceValue = int.Parse(distanceElement["distance"]["value"].ToString());
                distances.Add((distanceValue, index));
                index++;
            }

            // Sort distances based on proximity to origin
            distances.Sort((x, y) => x.distance.CompareTo(y.distance));

            // Populate closeProximity list with profiles sorted by distance
            foreach (var distance in distances)
            {
                var profileAtIndex = profiles.ElementAtOrDefault(distance.index);
                if (profileAtIndex != null)
                {
                    closeProximity.Add(profileAtIndex);
                }
            }
        }

        return closeProximity;
    }
}

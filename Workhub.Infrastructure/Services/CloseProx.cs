using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;
using Workhub.Application.Interfaces.Services;
using Workhub.Application.Common.Models;
using Workhub.Domain.Entities;

namespace Workhub.Infrastructure.Services;
public class CloseProx : ICloseProx
{
    public record FullAddress(string Country, string State, string Address);
    public async Task<dynamic> GetFullAddress(string longlat)
    {
        IConfigurationRoot configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
        List<Profile> closeProximity = new List<Profile>(); // Initialize list
        string apiKey = configuration.GetSection("GoogleApiKey").Value;
        var httpcon = new HttpClient();
        string baseurl = configuration.GetSection("GoogleGeoCode").Value;
        // Construct the API request URL
        string apiUrl = $"{baseurl}latlng={longlat}&key={apiKey}";

        // Create an instance of HttpClient
        using (var httpClient = new HttpClient())
        {
            try
            {
                // Send the HTTP request and get the response
                var response = await httpClient.GetAsync(apiUrl);
                // Ensure the request was successful
                if (response.IsSuccessStatusCode)
                {
                    // Parse the JSON response
                    var jsonResponse = await response.Content.ReadAsStringAsync();
                    var jsonObject = JObject.Parse(jsonResponse);

                    // Extract the address components
                    var results = jsonObject["results"];
                    if (results != null && results.HasValues)
                    {
                        var firstResult = results[0];
                        var addressComponents = firstResult["address_components"];

                        // Extract the country, state, and formatted address
                        string country = null, state = null, formattedAddress = null;
                        foreach (var component in addressComponents)
                        {
                            var types = component["types"];
                            if (types != null && types.HasValues)
                            {
                                foreach (var type in types)
                                {
                                    if (type.ToString() == "country")
                                    {
                                        country = component["long_name"].ToString();
                                    }
                                    else if (type.ToString() == "administrative_area_level_1")
                                    {
                                        state = component["long_name"].ToString();
                                    }
                                }
                            }
                        }

                        formattedAddress = firstResult["formatted_address"].ToString();
                        var resultdetails = new FullAddress(country, state, formattedAddress);
                        return resultdetails;
                    }
                    else
                    {

                        Console.WriteLine("No results found.");
                        return null;
                    }
                }
                else
                {
                    Console.WriteLine($"Failed to retrieve data. Status code: {response.StatusCode}");
                    return null;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
                return null;
            }
        }
    }

    public async Task<List<ProfileResponse>> GetProfilesSortedByProximity(string origin, string destinations, IEnumerable<ProfileResponse> profiles)
    {
        IConfigurationRoot configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json").Build();
        List<ProfileResponse> closeProximity = new List<ProfileResponse>(); // Initialize list
        string apiKey = configuration.GetSection("GoogleApiKey").Value;
        var httpcon = new HttpClient();
        string baseurl = configuration.GetSection("GoogleUrl").Value;
        string url = $"{baseurl}?origins={origin}&destinations={destinations}&key={apiKey}";
        HttpResponseMessage response = await httpcon.GetAsync(url);

        if (response.IsSuccessStatusCode)
        {
            string details = await response.Content.ReadAsStringAsync();
            JObject json = JObject.Parse(details);
            var distanceElements = json["rows"]![0]!["elements"]!.Children();

            // Create a list to store distances and corresponding indices
            List<(int distance, int index)> distances = new List<(int distance, int index)>();

            int index = 0;

            foreach (var distanceElement in distanceElements)
            {
                var distanceValue = int.Parse(distanceElement["distance"]!["value"]!.ToString());
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

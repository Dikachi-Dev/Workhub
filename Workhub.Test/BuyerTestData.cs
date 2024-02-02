using Workhub.Domain.Entities;

namespace Workhub.Test;

public class BuyerTestData
{
    public List<Profile> GetSampleBuyerProfiles()
    {
        return new List<Profile>
        {
            new Profile
            {
                FirstName = "John",
                LastName = "Doe",
                Email = "john.doe@example.com",
                PhoneNumber = "1234567890",
                Address = "123 Main St",
                State = "CA",
                Country = "US",
                Password = "securePassword"
            },
            new Profile
            {
                FirstName = "Alice",
                LastName = "Smith",
                Email = "alice.smith@example.com",
                PhoneNumber = "9876543210",
                Address = "456 Oak St",
                State = "NY",
                Country = "US",
                Password = "anotherPassword"
            },
                  new Profile
            {
                FirstName = "Sam",
                LastName = "Wills",
                Email = "same.smith@example.com",
                PhoneNumber = "9876547610",
                Address = "456 Oak St",
                State = "NY",
                Country = "US",
                Password = "anotherPassword1"
            },
            // Add more profiles as needed
        };
    }
}


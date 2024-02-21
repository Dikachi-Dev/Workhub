using Moq;
using Workhub.Application.Interfaces.Persistance;
using Workhub.Domain.Entities;

namespace Workhub.Test;

public class Tests
{
    private Mock<IProfileRepository> contextMock;
    private BuyerTestData testData;
    private IProfileRepository? MockRepository;
    private CancellationToken token;


    [SetUp]
    public void Setup()
    {
        contextMock = new Mock<IProfileRepository>();
        // Uncomment the line below if IBuyerProfileRepository extends IGenericRepository<BuyerProfile>
        contextMock.As<IGenericRepository<Profile>>();
        // contextMock.Setup(mr => mr.Add(It.IsAny<BuyerProfile>())); // If you extend IGenericRepository
        // contextMock.Setup(mr => mr.SaveChanges()); // If you extend IGenericRepository
        // buyerProfileRepository = new BuyerProfileRepository(contextMock.Object);
        testData = new BuyerTestData();
    }

    [Test]
    public void Test_AddBuyer()
    {
        // Arrange
        var profiles = testData.GetSampleBuyerProfiles().AsQueryable();
        var profile = new Profile
        {
            FirstName = "John",
            LastName = "Doe",
            Email = "john.doe@example.com",
            PhoneNumber = "1234567890",
            Address = "123 Main St",
            State = "CA",
            Country = "US",
            Password = "securePassword"
        };

        // Setup the mock repository
        contextMock.Setup(mr => mr.GetAll(token)).Returns(profiles);
        contextMock.Setup(mr => mr.Add(It.IsAny<Profile>(), token)); // Mock the Add method
        contextMock.Setup(mr => mr.SaveChanges(token));

        this.MockRepository = contextMock.Object;

        // Act
        MockRepository.Add(profile, token);
        MockRepository.SaveChanges(token);

        // Assert
        contextMock.Verify(mr => mr.Add(It.IsAny<Profile>(), token), Times.Once); // Verify that the Add method was called

        int profileCount = MockRepository.GetAll(token).Count();
        Assert.That(profileCount, Is.EqualTo(profiles.Count() + 1)); // Verify that the profile was added
    }



    [Test]
    public void Test_GetBuyerFilter()
    {
        // Arrange
        var filter = "john.doe@example.com";

        var profiles = testData.GetSampleBuyerProfiles().AsQueryable();
        contextMock.Setup(mr => mr.GetAll(token)).Returns(profiles);

        // return a product by Id
        contextMock.Setup(mr => mr.GetProfileByEmail(
            It.IsAny<string>(), token)).Returns((string i) => profiles.Where(
            x => x.Email == i).Single());

        this.MockRepository = contextMock.Object;



        // Act
        var result = MockRepository.GetProfileByEmail(filter, token);
        var result1 = MockRepository.GetAll(token);

        // Assert
        Assert.IsNotNull(result1);
        Assert.IsInstanceOf<Profile>(result);
        // Add more assertions based on the expected behavior of GetBuyerFilter
    }



}

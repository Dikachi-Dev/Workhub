using Moq;
using Workhub.Application.Interfaces.Persistance;
using Workhub.Domain.Entities;
using Workhub.Infrastructure.Data.Context;
using Workhub.Infrastructure.Persistance;

namespace Workhub.Test;

public class Tests
{
    private Mock<IBuyerProfileRepository> contextMock;
    private BuyerProfileRepository buyerProfileRepository;
    private AppDataContext context;
    private BuyerTestData testData;
    private IBuyerProfileRepository MockBuyerRepository;


    [SetUp]
    public void Setup()
    {
        contextMock = new Mock<IBuyerProfileRepository>();
        // buyerProfileRepository = new BuyerProfileRepository(contextMock.Object);
        testData = new BuyerTestData();
    }
    [Test]
    public void Test_AddBuyer()
    {
        // Arrange
        var profiles = testData.GetSampleBuyerProfiles().AsQueryable();
        var profile = new BuyerProfile
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
        contextMock.Setup(mr => mr.GetAll()).Returns(profiles);
        contextMock.Setup(mr => mr.Add(It.IsAny<BuyerProfile>()));

        this.MockBuyerRepository = contextMock.Object;

        // Act
        MockBuyerRepository.Add(profile);
        MockBuyerRepository.SaveChanges();

        // Assert
        contextMock.Verify(mr => mr.Add(It.IsAny<BuyerProfile>()), Times.Once); // Verify that the Add method was called
        contextMock.Verify(mr => mr.SaveChanges(), Times.Once); // Verify that SaveChanges method was called

        int profileCount = MockBuyerRepository.GetAll().Count();
        Assert.That(profileCount, Is.EqualTo(profiles.Count() + 1)); // Verify that the profile was added
    }


    [Test]
    public void Test_GetBuyerFilter()
    {
        // Arrange
        var filter = "john.doe@example.com";

        var profiles = testData.GetSampleBuyerProfiles().AsQueryable();
        contextMock.Setup(mr => mr.GetAll()).Returns(profiles);

        // return a product by Id
        contextMock.Setup(mr => mr.GetBuyerProfileByEmail(
            It.IsAny<string>())).Returns((string i) => profiles.Where(
            x => x.Email == i).Single());

        this.MockBuyerRepository = contextMock.Object;



        // Act
        var result = MockBuyerRepository.GetBuyerProfileByEmail(filter);
        var result1 = MockBuyerRepository.GetAll();

        // Assert
        Assert.IsNotNull(result1);
        Assert.IsInstanceOf<BuyerProfile>(result);
        // Add more assertions based on the expected behavior of GetBuyerFilter
    }



}

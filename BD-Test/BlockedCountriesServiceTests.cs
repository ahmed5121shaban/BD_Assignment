using Moq;
using BD_Modals;
using BD_Services;
using Repository;
using Xunit;
using BD_Dtos;
namespace BD_Test
{
    public class BlockedCountriesServiceTests
    {
        [Fact]
        public void AddBlockedCountry_ShouldAddCountry()
        {
            // Arrange
            var mockRepository = new Mock<IBlockedCountryRepository>();
            var mockIpApiService = new Mock<IIpApiService>();

            var service = new BlockedCountriesService(mockRepository.Object, mockIpApiService.Object);
            var ip = "123.45.67.89";
            var country = new IpApiDto() 
            { Country= "United States", City= "New York",Currency = new Currency{Name = "USD",Code = "",Symbol = "$"}, Ip=ip,CountryCode= "US" ,Org= "Google LLC",Region= "NY" };

            mockIpApiService.Setup(x => x.GetCountryByIpAsync(ip)).ReturnsAsync(country);
            mockRepository.Setup(x => x.AddBlockedCountry(It.IsAny<BlockedCountry>())).Returns(true);

            // Act
            var result = service.AddBlockedCountry(ip);

            // Assert
            Assert.True(result);
            mockRepository.Verify(x => x.AddBlockedCountry(It.IsAny<BlockedCountry>()), Times.Once);
        }

        [Fact]
        public void AddBlockedCountry_ShouldNotAddDuplicate()
        {
            // Arrange
            var mockRepository = new Mock<IBlockedCountryRepository>();
            var mockIpApiService = new Mock<IIpApiService>();

            var service = new BlockedCountriesService(mockRepository.Object, mockIpApiService.Object);
            var ip = "123.45.67.89";
            var country = new IpApiDto()
            { Country = "United States", City = "New York", Currency = new Currency { Name = "USD", Code = "", Symbol = "$" }, Ip = ip, CountryCode = "US", Org = "Google LLC", Region = "NY" };
            mockIpApiService.Setup(x => x.GetCountryByIpAsync(ip)).ReturnsAsync(country);
            mockRepository.Setup(x => x.AddBlockedCountry(It.IsAny<BlockedCountry>())).Returns(false);

            // Act
            var result = service.AddBlockedCountry(ip);

            // Assert
            Assert.False(result);
            mockRepository.Verify(x => x.AddBlockedCountry(It.IsAny<BlockedCountry>()), Times.Once);
        }
        [Fact]
        public void RemoveBlockedCountry_ShouldRemoveCountry()
        {
            // Arrange
            var mockRepository = new Mock<IBlockedCountryRepository>();
            var mockIpApiService = new Mock<IIpApiService>();

            var service = new BlockedCountriesService(mockRepository.Object, mockIpApiService.Object);
            var ip = "123.45.67.89";

            mockRepository.Setup(x => x.RemoveBlockedCountry(ip)).Returns(true);

            // Act
            var result = service.RemoveBlockedCountry(ip);

            // Assert
            Assert.True(result);
            mockRepository.Verify(x => x.RemoveBlockedCountry(ip), Times.Once);
        }

        [Fact]
        public void RemoveBlockedCountry_ShouldReturnFalseIfNotFound()
        {
            // Arrange
            var mockRepository = new Mock<IBlockedCountryRepository>();
            var mockIpApiService = new Mock<IIpApiService>();

            var service = new BlockedCountriesService(mockRepository.Object, mockIpApiService.Object);
            var ip = "123.45.67.89";

            mockRepository.Setup(x => x.RemoveBlockedCountry(ip)).Returns(false);

            // Act
            var result = service.RemoveBlockedCountry(ip);

            // Assert
            Assert.False(result);
            mockRepository.Verify(x => x.RemoveBlockedCountry(ip), Times.Once);
        }
        [Fact]
        public void VerifyBlockedCountry_ShouldReturnTrueIfBlocked()
        {
            // Arrange
            var mockRepository = new Mock<IBlockedCountryRepository>();
            var mockIpApiService = new Mock<IIpApiService>();

            var service = new BlockedCountriesService(mockRepository.Object, mockIpApiService.Object);
            var ip = "123.45.67.89";
            var country = new IpApiDto()
            { Country = "United States", City = "New York", Currency = new Currency { Name = "USD", Code = "", Symbol = "$" }, Ip = ip, CountryCode = "US", Org = "Google LLC", Region = "NY" };
            mockIpApiService.Setup(x => x.GetCountryByIpAsync(ip)).ReturnsAsync(country);
            mockRepository.Setup(x => x.IsBlocked(ip)).Returns(true);

            // Act
            var result = service.VerifyBlockedCountry(ip);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void VerifyBlockedCountry_ShouldReturnFalseIfNotBlocked()
        {
            // Arrange
            var mockRepository = new Mock<IBlockedCountryRepository>();
            var mockIpApiService = new Mock<IIpApiService>();

            var service = new BlockedCountriesService(mockRepository.Object, mockIpApiService.Object);
            var ip = "123.45.67.89";
            var country = new IpApiDto()
            { Country = "United States", City = "New York", Currency = new Currency { Name = "USD", Code = "", Symbol = "$" }, Ip = ip, CountryCode = "US", Org = "Google LLC", Region = "NY" };
            mockIpApiService.Setup(x => x.GetCountryByIpAsync(ip)).ReturnsAsync(country);
            mockRepository.Setup(x => x.IsBlocked(ip)).Returns(false);

            // Act
            var result = service.VerifyBlockedCountry(ip);

            // Assert
            Assert.False(result);
        }
        [Fact]
        public void AddTemporalBlock_ShouldAddTemporalBlock()
        {
            // Arrange
            var mockRepository = new Mock<IBlockedCountryRepository>();
            var mockIpApiService = new Mock<IIpApiService>();

            var service = new BlockedCountriesService(mockRepository.Object, mockIpApiService.Object);
            var countryCode = "US";
            var durationMinutes = 120;

            mockRepository.Setup(x => x.AddTemporalBlock(countryCode, durationMinutes)).Returns(2);

            // Act
            var result = service.AddTemporalBlock(countryCode, durationMinutes);

            // Assert
            Assert.Equal(2, result);
            mockRepository.Verify(x => x.AddTemporalBlock(countryCode, durationMinutes), Times.Once);
        }

        [Fact]
        public void AddTemporalBlock_ShouldNotAddDuplicate()
        {
            // Arrange
            var mockRepository = new Mock<IBlockedCountryRepository>();
            var mockIpApiService = new Mock<IIpApiService>();

            var service = new BlockedCountriesService(mockRepository.Object, mockIpApiService.Object);
            var countryCode = "US";
            var durationMinutes = 120;

            mockRepository.Setup(x => x.AddTemporalBlock(countryCode, durationMinutes)).Returns(0);

            // Act
            var result = service.AddTemporalBlock(countryCode, durationMinutes);

            // Assert
            Assert.Equal(0, result);
            mockRepository.Verify(x => x.AddTemporalBlock(countryCode, durationMinutes), Times.Once);
        }
    }
}
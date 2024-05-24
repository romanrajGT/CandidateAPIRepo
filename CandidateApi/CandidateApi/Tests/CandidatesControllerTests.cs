using CandidateApi.Controllers;
using CandidateApi.Models;
using CandidateApi.Services;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace CandidateApi.Tests
{
    public class CandidatesControllerTests
    {
        [Fact]
        public async Task AddOrUpdateCandidate_ValidCandidate_ReturnsOkResult()
        {
            // Arrange
            var mockService = new Mock<ICandidateService>();
            var controller = new CandidatesController(mockService.Object);
            var candidate = new Candidate { FirstName = "Roman", LastName = "Bjr", Email = "roman.bjr@ex.com", Comment = "Test Comment" };

            // Act
            var result = await controller.AddOrUpdateCandidate(candidate);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var value = okResult.Value as dynamic;
            Assert.Equal("Candidate added or updated successfully.", value.message);
        }

        [Fact]
        public async Task AddOrUpdateCandidate_MissingFirstName_ReturnsBadRequest()
        {
            // Arrange
            var mockService = new Mock<ICandidateService>();
            var controller = new CandidatesController(mockService.Object);
            var candidate = new Candidate { LastName = "Bjr", Email = "roman.bjr@ex.com", Comment = "Test Comment" };

            // Act
            var result = await controller.AddOrUpdateCandidate(candidate);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            dynamic value = badRequestResult.Value;
            Assert.True(value.ContainsKey("FirstName"));
            Assert.Equal("First name is required.", value["FirstName"][0]);
        }

        [Fact]
        public async Task AddOrUpdateCandidate_MissingLastName_ReturnsBadRequest()
        {
            // Arrange
            var mockService = new Mock<ICandidateService>();
            var controller = new CandidatesController(mockService.Object);
            var candidate = new Candidate { FirstName = "Roman", Email = "roman.bjr@ex.com", Comment = "Test Comment" };

            // Act
            var result = await controller.AddOrUpdateCandidate(candidate);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            dynamic value = badRequestResult.Value;
            Assert.True(value.ContainsKey("LastName"));
            Assert.Equal("Last name is required.", value["LastName"][0]);
        }

        [Fact]
        public async Task AddOrUpdateCandidate_MissingEmail_ReturnsBadRequest()
        {
            // Arrange
            var mockService = new Mock<ICandidateService>();
            var controller = new CandidatesController(mockService.Object);
            var candidate = new Candidate { FirstName = "Roman", LastName = "Bjr", Comment = "Test Comment" };

            // Act
            var result = await controller.AddOrUpdateCandidate(candidate);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            dynamic value = badRequestResult.Value;
            Assert.True(value.ContainsKey("Email"));
            Assert.Equal("Email is required.", value["Email"][0]);
        }

        [Fact]
        public async Task AddOrUpdateCandidate_MissingComment_ReturnsBadRequest()
        {
            // Arrange
            var mockService = new Mock<ICandidateService>();
            var controller = new CandidatesController(mockService.Object);
            var candidate = new Candidate { FirstName = "Roman", LastName = "Bjr", Email = "roman.bjr@ex.com" };

            // Act
            var result = await controller.AddOrUpdateCandidate(candidate);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            dynamic value = badRequestResult.Value;
            Assert.True(value.ContainsKey("Comment"));
            Assert.Equal("Comment is required.", value["Comment"][0]);
        }
    }
}

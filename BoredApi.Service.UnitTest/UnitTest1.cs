using BoredApi.Service.Repository;
using BoredApi.Service.Services;
using FunctionApp1.Models.Dto;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Xunit;
using Assert = Microsoft.VisualStudio.TestTools.UnitTesting.Assert;

namespace BoredApi.Service.UnitTest
{
    [TestClass]
    public class UnitTest1
    {
        private Mock<IActibityTableRepository> _mockRepository;
        private IBoredAPIManagerService _service;


        public UnitTest1()
        {
            _mockRepository = new Mock<IActibityTableRepository>();
            _service = new BoredAPIManagerService(_mockRepository.Object);
        }


        [Fact]
        public async void AddActvity()
        {
            ActivityDto ac = new ActivityDto
            {
                key = "2896176",
                type = "busywork",
                price = 0,
                PartitionKey = "Activity",
                RowKey = "2896176",
                accessibility = 0.08,
                participants = 1,
            };
            _mockRepository.Setup(p => p.AddAsync(ac)).ReturnsAsync(ac);
            //Act
            var result = await _service.AddAsync(ac);
            //Assert
            Assert.IsNotNull(result);
            Assert.AreNotEqual(0, result.key);
        }

        [Fact]
        public async void UpdateActvity()
        {
            ActivityDto ac = new ActivityDto
            {
                key = "2896176",
                type = "busywork",
                price = 0,
                PartitionKey = "Activity",
                RowKey = "2896176",
                accessibility = 1.08,
                participants = 2,
            };
            var activity = ac;
            activity.accessibility = 3;
            _mockRepository.Setup(p => p.UpdateAsync(ac)).ReturnsAsync(activity);
            //Act
            var result = await _service.UpdateAsync(ac);
            //Assert
            Assert.AreEqual("2896176", result.key);
            Assert.AreEqual(3, result.accessibility);
        }

        [Fact]
        public async void GetEmployee()
        {
            //Arrange
            string id = "2896176";
            _mockRepository.Setup(p => p.GetByIdAsync("2896176")).ReturnsAsync(new ActivityDto { type = "education" });

            //Act
            var result = await _service.GetByIdAsync(id);

            //Assert
            Assert.IsNotNull(result);
        }

        [Fact]
        public async void DeleteEmployee()
        {
            //Arrange
            ActivityDto ac = new ActivityDto
            {
                key = "2896176",
                type = "busywork",
                price = 0,
                PartitionKey = "Activity",
                RowKey = "2896176",
                accessibility = 0.08,
                participants = 2,
            };

            var result2 = await _service.DeleteAsync(ac);
            //Assert
            Assert.IsTrue(result2);
        }
    }
}
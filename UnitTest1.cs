using Poliza.Models;
using Poliza.Repositories;
using Poliza.Repositories.Interfaces;
using System.ComponentModel.DataAnnotations;
using FluentAssertions;
using Moq;

namespace TestPolicies
{
    public class Tests
    {
        [TestFixture]
        public class PolicyTests
        {
            [Test]
            public void Policy_Should_Not_Save_If_CustomerName_Is_Null()
            {
                var policy = new PolicyEntity
                {
                    ID = 1,
                    PolicyNumber = 100,
                    CustomerName = null,
                    CustomerID = "123456",
                    CustomerBirthDay = new DateTime(2000, 1, 1),
                    PolicyCoverage = "Full",
                    PolicyCoverageValue = 5000,
                    PolicyPlanName = "Plan A",
                    CustomerCity = "New York",
                    CustomerAddress = "123 Main St",
                    VehiclePlate = "ABC123",
                    VehicleModel = "Ford",
                    VehicleIsCheked = true,
                    PolicyStartDate = new DateTime(2022, 1, 1),
                    PolicyEndDate = new DateTime(2023, 1, 1)
                };

                // Act & Assert
                Assert.Throws<ValidationException>(() => Validator.ValidateObject(policy, new ValidationContext(policy), true));
            }

            [Test]
            public void Policy_Should_Not_Save_If_Policy_Is_Not_Valid()
            {
                var policy = new PolicyEntity
                {
                    ID = 1,
                    PolicyNumber = 100,
                    CustomerName = "Tatiana",
                    CustomerID = "123456",
                    CustomerBirthDay = new DateTime(2000, 1, 1),
                    PolicyCoverage = "Full",
                    PolicyCoverageValue = 5000,
                    PolicyPlanName = "Plan A",
                    CustomerCity = "New York",
                    CustomerAddress = "123 Main St",
                    VehiclePlate = "ABC123",
                    VehicleModel = "Ford",
                    VehicleIsCheked = true,
                    PolicyStartDate = new DateTime(2022, 1, 1),
                    PolicyEndDate = new DateTime(2023, 1, 1)
                };
                IPolicyRepository policyRepository = new PolicyRepository();

                // Act 
                Func<Task> act = async () => await policyRepository.PostPolicy(policy);

                // Assert
                 act.Should().ThrowAsync<InvalidOperationException>()
                    .WithMessage("La poliza no esta vigente");
            }


            [Test]
            public async Task Login_Returns_Null_If_User_Does_Not_Exist()
            {
                var user = new UserEntity ( 1, "testuser","testpassword","Administrador");

                var mockLoginRepo = new Mock<ILoginRepository>();
                mockLoginRepo.Setup(repo => repo.GetUser(user.Name, user.Password)).ReturnsAsync((UserEntity)null);

                var result = await mockLoginRepo.Object.GetUser(user.Name, user.Password);

                // Assert
                Assert.IsNull(result);
            }
        }
    }
}
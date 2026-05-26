using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Moq;
using Refit;
using Xunit;
using Mono.Core;
using Mono.Core.Customers;

namespace Mono.Core.Customers.Tests
{
    public class CustomerServiceTests
    {
        private readonly Mock<ICustomerService> _mockRefit;
        private readonly CustomerService _service;

        public CustomerServiceTests()
        {
            _mockRefit = new Mock<ICustomerService>();
            var builder = new Mock<IRefitClientBuilder<ICustomerService>>();
            builder.Setup(x => x.Build(It.IsAny<string>())).Returns(_mockRefit.Object);
            _service = new CustomerService(builder.Object);
        }

        private static ApiResponse<MonoStandardResponse<T>> Ok<T>(T data) =>
            new ApiResponse<MonoStandardResponse<T>>(
                new HttpResponseMessage(HttpStatusCode.OK),
                new MonoStandardResponse<T> { Data = data, Status = "successful", Success = true },
                new RefitSettings());

        [Fact]
        public async Task CreateIndividualCustomer_ShouldUnwrapResponse()
        {
            var model = new CreateIndividualCustomerModel
            {
                FirstName = "Ada",
                LastName = "Lovelace",
                Email = "ada@example.com",
                Phone = "+2348012345678",
                Identity = new CustomerIdentity
                {
                    Type = CustomerIdentityTypeConstants.Bvn,
                    Number = "12345678901",
                },
            };
            var expected = new CustomerResponse
            {
                Id = "cust_5f8d9c",
                Email = "ada@example.com",
                Type = CustomerTypeConstants.Individual,
                FirstName = "Ada",
                LastName = "Lovelace",
                Phone = "+2348012345678",
                CreatedAt = DateTime.UtcNow,
            };

            _mockRefit.Setup(x => x.CreateIndividualCustomer(model, It.IsAny<CancellationToken>()))
                .ReturnsAsync(Ok(expected));

            var result = await _service.CreateIndividualCustomer(model);

            Assert.True(result.Success);
            Assert.Equal("cust_5f8d9c", result.Data.Id);
            Assert.Equal(CustomerTypeConstants.Individual, result.Data.Type);
        }

        [Fact]
        public async Task CreateBusinessCustomer_ShouldHitSameEndpointWithBusinessBody()
        {
            var model = new CreateBusinessCustomerModel
            {
                BusinessName = "Acme Ltd",
                Email = "ops@acme.example",
                Phone = "+2348099999999",
                Address = "12 Acme Way",
                Identity = new CustomerIdentity { Type = "rc_number", Number = "RC123456" },
            };
            var expected = new CustomerResponse
            {
                Id = "cust_biz_5f8d9c",
                BusinessName = "Acme Ltd",
                Type = CustomerTypeConstants.Business,
            };

            _mockRefit.Setup(x => x.CreateBusinessCustomer(model, It.IsAny<CancellationToken>()))
                .ReturnsAsync(Ok(expected));

            var result = await _service.CreateBusinessCustomer(model);

            Assert.Equal(CustomerTypeConstants.Business, result.Data.Type);
            Assert.Equal("Acme Ltd", result.Data.BusinessName);
        }

        [Fact]
        public async Task ListCustomers_DefaultsQueryOptionsWhenNull()
        {
            var expected = new CustomerListResponse
            {
                Customers = new List<CustomerResponse>
                {
                    new CustomerResponse { Id = "cust_1", FirstName = "Ada" },
                    new CustomerResponse { Id = "cust_2", FirstName = "Grace" },
                },
                Meta = new CustomerListMeta { Total = 2, Page = 1 },
            };

            _mockRefit.Setup(x => x.ListCustomers(It.IsAny<CustomerListQueryOptions>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(Ok(expected));

            var result = await _service.ListCustomers();

            Assert.Equal(2, result.Data.Customers.Count);
            _mockRefit.Verify(x => x.ListCustomers(It.IsNotNull<CustomerListQueryOptions>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task DeleteCustomer_ShouldRoundTripReference()
        {
            _mockRefit.Setup(x => x.DeleteCustomer("cust_5f8d9c", It.IsAny<CancellationToken>()))
                .ReturnsAsync(Ok<dynamic>(null));

            var result = await _service.DeleteCustomer("cust_5f8d9c");

            Assert.True(result.Success);
            _mockRefit.Verify(x => x.DeleteCustomer("cust_5f8d9c", It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}

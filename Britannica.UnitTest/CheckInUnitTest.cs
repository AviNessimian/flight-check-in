using Britannica.Application.Contracts;
using Britannica.Application.Interactors;
using Britannica.Domain.Entities;
using Britannica.Domain.Exceptions;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Britannica.UnitTest
{
    public class CheckInUnitTest
    {
        [Fact]
        public void CheckInInteractor_ValidateAircraftWeightLimit_ThrowsBusinessRuleException()
        {
            //arrange
            var cToken = CancellationToken.None;

            var logger = new NullLogger<CheckInInteractor>();
            var passengerRepositoryMock = Mock.Of<IPassengerRepository>();

            var flightRepositoryMock = new Mock<IFlightRepository>();
            var flight = Task.FromResult(new FlightEntity
            {
                Id = 1,
                Aircraft = new AircraftEntity
                {
                    WeightLimit = 12
                },
                PassengerFlights = new List<PassengerFlightEntity>()
                {
                    new PassengerFlightEntity
                    {
                        Baggages = new List<BaggageEntity>
                        {
                            new BaggageEntity
                            {
                                Weight = 6
                            },
                            new BaggageEntity
                            {
                                Weight = 6
                            }
                        }
                    }
                }
            });

            flightRepositoryMock
                .Setup(m => m.Get(1, cToken))
                .Returns(flight);

            var interactor = new CheckInInteractor(
                logger,
                flightRepositoryMock.Object,
                passengerRepositoryMock);

            var request = new CheckInRequest
            {
                FlightId = 1,
                PassengerId = 1,
                Baggages = new List<CheckInRequest.Baggage>
                {
                    new CheckInRequest.Baggage
                    {
                        Weight = 1
                    }
                },
            };

            //act
            Action act = () => interactor.Handle(request, cToken).GetAwaiter().GetResult();

            //assert
            var exception = Assert.Throws<BusinessRuleException>(act);

            //More detailed assertions.
            Assert.Equal($"Aircraft has a limited load weight of {flight.Result.Aircraft.WeightLimit}", exception.Message);

        }
    }
}

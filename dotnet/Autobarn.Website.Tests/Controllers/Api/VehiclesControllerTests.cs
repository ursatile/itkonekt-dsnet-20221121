using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Autobarn.Data;
using Autobarn.Data.Entities;
using Autobarn.Messages;
using Autobarn.Website.Controllers.api;
using Autobarn.Website.Models;
using EasyNetQ;
using EasyNetQ.Internals;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Shouldly;
using Xunit;

namespace Autobarn.Website.Tests.Controllers.Api {
    public class ModelsControllerTests {
        [Fact]
        public void POST_Publishes_Vehicle_To_Bus() {
            var db = new FakeDatabase();
            var bus = new FakeBus();
            var c = new ModelsController(db, bus);
            c.Post("test-test", new VehicleDto() {
                Registration = "TEST0002",
                Color = "Blue",
                Year = 1985
            });

            var bucket = ((FakePubSub) (bus.PubSub)).Bucket;
            bucket.Count.ShouldBe(1);
            var message = ((NewVehicleMessage) bucket[0]);
            message.Year.ShouldBe(1985);
        }
    }
    public class VehiclesControllerTests {
        [Fact]
        public void GET_Vehicle_Returns_NotFound() {
            // arrange
            var db = new FakeDatabase();
            var c = new VehiclesController(db);

            // act
            var result = c.Get("NOSUCHCAR");

            // assert
            result.ShouldBeOfType<NotFoundResult>();
        }

        [Fact]
        public void GET_Vehicle_Returns_Vehicle() {
            // arrange
            var db = new FakeDatabase();
            var c = new VehiclesController(db);

            // act
            var result = c.Get("TEST0001");

            // assert
            result.ShouldBeOfType<OkObjectResult>();
            var resource = (dynamic) ((OkObjectResult) result).Value;
            ((string) resource.Registration).ShouldBe("TEST0001");
        }
    }

    public class FakePubSub : IPubSub {
        public ArrayList Bucket { get; set; } = new ArrayList();
        public Task PublishAsync<T>(T message, Action<IPublishConfiguration> configure, CancellationToken cancellationToken = new CancellationToken()) {
            Bucket.Add(message);
            return Task.CompletedTask;
        }

        public AwaitableDisposable<SubscriptionResult> SubscribeAsync<T>(string subscriptionId, Func<T, CancellationToken, Task> onMessage, Action<ISubscriptionConfiguration> configure,
            CancellationToken cancellationToken = new CancellationToken()) {
            throw new NotImplementedException();
        }
    }
    public class FakeBus : IBus {
        private IPubSub pubSub;
        public FakeBus() {
            this.pubSub = new FakePubSub();
        }
        public void Dispose() { }

        public IPubSub PubSub => pubSub;

        public IRpc Rpc { get; }
        public ISendReceive SendReceive { get; }
        public IScheduler Scheduler { get; }
        public IAdvancedBus Advanced { get; }
    }
    public class FakeDatabase : IAutobarnDatabase {
        public int CountVehicles() {
            throw new System.NotImplementedException();
        }

        public IEnumerable<Vehicle> ListVehicles() {
            throw new System.NotImplementedException();
        }

        public IEnumerable<Manufacturer> ListManufacturers() {
            throw new System.NotImplementedException();
        }

        public IEnumerable<Model> ListModels() {
            throw new System.NotImplementedException();
        }

        public Vehicle FindVehicle(string registration) {
            if (registration == "TEST0001")
                return new Vehicle() {
                    Registration = registration
                };
            return null;
        }

        public Model FindModel(string code) {
            return new Model();
        }

        public Manufacturer FindManufacturer(string code) {
            throw new System.NotImplementedException();
        }

        public void CreateVehicle(Vehicle vehicle) {
            
        }

        public void UpdateVehicle(Vehicle vehicle) {
            throw new System.NotImplementedException();
        }

        public void DeleteVehicle(Vehicle vehicle) {
            throw new System.NotImplementedException();
        }
    }
}

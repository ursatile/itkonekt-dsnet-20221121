using System.Collections.Generic;
using Autobarn.Data;
using Autobarn.Data.Entities;
using Autobarn.Website.Controllers.api;
using Microsoft.AspNetCore.Mvc;
using Shouldly;
using Xunit;

namespace Autobarn.Website.Tests.Controllers.Api {
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
            throw new System.NotImplementedException();
        }

        public Manufacturer FindManufacturer(string code) {
            throw new System.NotImplementedException();
        }

        public void CreateVehicle(Vehicle vehicle) {
            throw new System.NotImplementedException();
        }

        public void UpdateVehicle(Vehicle vehicle) {
            throw new System.NotImplementedException();
        }

        public void DeleteVehicle(Vehicle vehicle) {
            throw new System.NotImplementedException();
        }
    }
}

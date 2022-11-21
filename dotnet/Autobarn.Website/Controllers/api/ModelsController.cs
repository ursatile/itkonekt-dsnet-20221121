using System;
using Autobarn.Data;
using Autobarn.Data.Entities;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Autobarn.Messages;
using Autobarn.Website.Models;
using EasyNetQ;

namespace Autobarn.Website.Controllers.api {
	[Route("api/[controller]")]
	[ApiController]
	public class ModelsController : ControllerBase {
		private readonly IAutobarnDatabase db;
        private readonly IBus bus;

        public ModelsController(IAutobarnDatabase db, IBus bus) {
            this.db = db;
            this.bus = bus;
        }

		[HttpGet]
		public IEnumerable<Model> Get() {
			return db.ListModels();
		}

		[HttpGet("{id}")]
		public IActionResult Get(string id) {
			var vehicleModel = db.FindModel(id);
			if (vehicleModel == default) return NotFound();
            return Ok(vehicleModel.ToResource());
		}


        // POST api/vehicles
        [HttpPost("{id}")]
        public IActionResult Post(string id, [FromBody] VehicleDto dto) {
            var vehicleModel = db.FindModel(id);
            if (vehicleModel == default) return NotFound($"We don't have any vehicle model with the code {id}");

            var existing = db.FindVehicle(dto.Registration);
            if (existing != default)
                return Conflict(
                    $"We already have the vehicle {dto.Registration} listed for sale, and you can't sell the same car twice.");
            var vehicle = new Vehicle {
                Registration = dto.Registration,
                Color = dto.Color,
                Year = dto.Year,
                VehicleModel = vehicleModel
            };

            PublishNewVehicleMessage(vehicle);
            db.CreateVehicle(vehicle);

            return Created($"/api/vehicles/{vehicle.Registration}", vehicle.ToResource());
        }

        private void PublishNewVehicleMessage(Vehicle vehicle) {
            var m = new NewVehicleMessage {
                Registration = vehicle.Registration,
                ManufacturerName = vehicle.VehicleModel.Manufacturer.Name,
                ModelName = vehicle.VehicleModel.Name,
                Color = vehicle.Color,
                Year = vehicle.Year,
                ListedAt = DateTimeOffset.UtcNow
            };
            bus.PubSub.Publish(m);
        }
	}
}

using System;
using Autobarn.Data;
using Autobarn.Data.Entities;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using Autobarn.Messages;
using Autobarn.Website.Models;
using Castle.Core.Logging;
using EasyNetQ;
using Microsoft.Extensions.Logging;

namespace Autobarn.Website.Controllers.api {
    [Route("api/[controller]")]
	[ApiController]
	public class ModelsController : ControllerBase {
		private readonly IAutobarnDatabase db;
        private readonly IBus bus;
        private readonly ILogger<ModelsController> logger;

        public ModelsController(IAutobarnDatabase db, IBus bus, ILogger<ModelsController> logger) {
            this.db = db;
            this.bus = bus;
            this.logger = logger;
        }

		[HttpGet]
		public IActionResult Get() {
			return Ok(db.ListModels().Select(m => m.ToResource()));
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
                ManufacturerName = vehicle?.VehicleModel?.Manufacturer?.Name ?? "UNKNOWN",
                ModelName = vehicle?.VehicleModel?.Name ?? "UNKNOWN",
                Color = vehicle.Color,
                Year = vehicle.Year,
                ListedAt = DateTimeOffset.UtcNow
            };
            logger.LogInformation($"Publishing NewVehicleMessage: {m}");
            bus.PubSub.Publish(m);
        }
	}
}

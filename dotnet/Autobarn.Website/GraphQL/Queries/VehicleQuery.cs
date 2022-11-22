using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Autobarn.Data;
using Autobarn.Data.Entities;
using Autobarn.Website.GraphQL.GraphTypes;
using GraphQL;
using GraphQL.Types;

namespace Autobarn.Website.GraphQL.Queries {
    public sealed class VehicleQuery : ObjectGraphType {
        private readonly IAutobarnDatabase db;

        public VehicleQuery(IAutobarnDatabase db) {
            this.db = db;
            Field<ListGraphType<VehicleGraphType>>("Vehicles")
                .Description("Return all vehicles in the system")
                .Resolve(GetAllVehicles);

            Field<VehicleGraphType>("Vehicle")
                .Description("Return details of a single vehicle")
                .Arguments(MakeNonNullStringArgument("registration", "The registration of the vehicle you want"))
                .Resolve(GetVehicle);


            Field<ListGraphType<VehicleGraphType>>("VehiclesByColor")
                .Description("Return all vehicles matching a specified color")
                .Arguments(MakeNonNullStringArgument("color", "What color car do you want?"))
                .Resolve(GetVehiclesByColor);

        }

        private object GetVehiclesByColor(IResolveFieldContext<object> context) {
            var color = context.GetArgument<string>("color");
            return db.ListVehicles().Where(v => v.Color.Contains(color, StringComparison.InvariantCultureIgnoreCase));
        }

        private object GetVehicle(IResolveFieldContext<object> context) {
            var registration = context.GetArgument<string>("registration");
            return db.FindVehicle(registration);
        }

        private QueryArgument MakeNonNullStringArgument(string name, string description) {
            return new QueryArgument<NonNullGraphType<StringGraphType>> {
                Name = name, Description = description
            };
        }

        private IEnumerable<Vehicle> GetAllVehicles(IResolveFieldContext<object> arg) => db.ListVehicles();
    }
}

using Autobarn.Data.Entities;
using GraphQL.Types;

namespace Autobarn.Website.GraphQL.GraphTypes {
    public sealed class VehicleGraphType : ObjectGraphType<Vehicle> {
        public VehicleGraphType() {
            Name = "vehicle";
            Field(c => c.VehicleModel, nullable: false, type: typeof(ModelGraphType))
                .Description("The model of this particular vehicle");
            Field(c => c.Registration);
            Field(c => c.Color);
            Field(c => c.Year);
        }
    }

    public sealed class ModelGraphType : ObjectGraphType<Model> {
        public ModelGraphType() {
            Name = "model";
            Field(m => m.Name).Description("The name of this model, e.g. Golf, Beetle, 5 Series, Model X");
            Field(m => m.Manufacturer, type: typeof(ManufacturerGraphType)).Description("The make of this model of car");
        }
    }

    public sealed class ManufacturerGraphType : ObjectGraphType<Manufacturer> {
        public ManufacturerGraphType() {
            Name = "manufacturer";
            Field(c => c.Name).Description("The name of the manufacturer, e.g. Tesla, Volkswagen, Ford");
        }
    }
}

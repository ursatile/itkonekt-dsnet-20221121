using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Dynamic;
using System.Linq;
using Autobarn.Data.Entities;

namespace Autobarn.Website.Controllers {
    public static class HypermediaExtensions {
        public static dynamic ToDynamic(this object value, object links) {
            IDictionary<string, object> expando = new ExpandoObject();
            expando.Add("_links", links);
            var properties = TypeDescriptor.GetProperties(value.GetType());
            foreach (PropertyDescriptor property in properties) {
                if (Ignore(property)) continue;
                expando.Add(property.Name, property.GetValue(value));
            }
            return expando;
        }

        public static dynamic ToResource(this Vehicle vehicle) {
            var links = new {
                self = new {
                    href = $"/api/vehicles/{vehicle.Registration}"
                },
                model = new {
                    href = $"/api/models/{vehicle.ModelCode}"
                }
            };
            return vehicle.ToDynamic(links);
        }

        private static bool Ignore(PropertyDescriptor property) {
            return property.Attributes.OfType<Newtonsoft.Json.JsonIgnoreAttribute>().Any();

        }
    }
}
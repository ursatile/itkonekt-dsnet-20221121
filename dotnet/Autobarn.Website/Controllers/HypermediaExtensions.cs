using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Dynamic;
using System.Linq;
using System.Runtime.InteropServices;
using Autobarn.Data.Entities;

namespace Autobarn.Website.Controllers {
    public static class HypermediaExtensions {
        public static dynamic ToDynamic(this object value, object links, object actions = null) {
            IDictionary<string, object> expando = new ExpandoObject();
            expando.Add("_links", links);
            if (actions != default) expando.Add("_actions", actions);
            var properties = TypeDescriptor.GetProperties(value.GetType());
            foreach (PropertyDescriptor property in properties) {
                if (Ignore(property)) continue;
                expando.Add(property.Name, property.GetValue(value));
            }
            return expando;
        }

        public static dynamic ToResource(this Model model) {
            var links = new {
                self = new {
                    href = $"/api/models/{model.Code}"
                }
            };
            var actions = new {
                create = new {
                    name = $"Create a new {model.Manufacturer.Name} {model.Name}",
                    href = $"/api/models/{model.Code}",
                    method = "POST",
                    type = "application/json"
                }
            };
            return model.ToDynamic(links, actions);

        }
        public static dynamic ToResource(this Vehicle vehicle) {
            var href = $"/api/vehicles/{vehicle.Registration}";
            var links = new {
                self = new {
                    href
                },
                model = new {
                    href = $"/api/models/{vehicle.ModelCode}"
                }
            };
            var actions = new {
                update = new {
                    name = "Update this vehicle",
                    type = "application/json",
                    href,
                    method = "PUT"
                },
                delete = new {
                    name = "Delete this vehicle",
                    href,
                    method = "DELETE"
                }
            };
            return vehicle.ToDynamic(links, actions);
        }

        private static bool Ignore(PropertyDescriptor property) {
            return property.Attributes.OfType<Newtonsoft.Json.JsonIgnoreAttribute>().Any();

        }
    }
}
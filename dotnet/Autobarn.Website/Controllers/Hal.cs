using System.Dynamic;

namespace Autobarn.Website.Controllers {
    public class Hal {
        public static dynamic Paginate(string url, int index, int total, int count) {
            dynamic links = new ExpandoObject();
            links.self = new {
                href = "/api/vehicles"
            };
            if (index > 0) {
                links.previous = new { href = $"/api/vehicles?index={index - count}" };
                links.first = new { href = $"/api/vehicles?index=0" };
            }
            if (index+1 < total) {
                links.next = new { href = $"/api/vehicles?index={index + count}" };
                links.final = new { href = $"/api/vehicles?index={total - total % count}" };
            }
            return links;
        }
    }
}
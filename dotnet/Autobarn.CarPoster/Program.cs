using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Autobarn.CarPoster {
    internal class Program {
        static readonly HttpClient client = new HttpClient();
        static Random random = new Random();

        static async Task Main(string[] args) {
            var uris = await GetModelUris(); 
            Console.WriteLine("Press any key to sell a random car...");
            while (true) {
                Console.WriteLine("Waiting...");
                Console.ReadKey(true);
                var uri = "https://workshop.ursatile.com:5001" + uris[random.Next(uris.Count)];
                var vehicle = new {
                    registration = Guid.NewGuid().ToString().Substring(0, 7),
                    color = GetRandomColor(),
                    Year = 1950 + random.Next(70)
                };
                var post = JsonConvert.SerializeObject(vehicle);
                Console.WriteLine(post);
                var postContent = new StringContent(post, Encoding.UTF8, "application/json");
                var postResponse = await client.PostAsync(uri,  postContent);
                Console.WriteLine(postResponse);
            }
        }

        private static async Task<List<string>> GetModelUris() {
            Console.WriteLine("Retrieving vehicle model URLs...");
            var json = await client.GetStringAsync("https://workshop.ursatile.com:5001/api/models");
            var models = JsonConvert.DeserializeObject<List<dynamic>>(json);
            return models.Select(model => (string) model._actions.create.href).ToList();
        }

        private static string[] colors = { "Red", "Green", "Blue", "Orange", "Purple", "Silver", "White", "Black" };
        static string GetRandomColor() => colors[random.Next(colors.Length)];
    }
}

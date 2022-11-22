using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Autobarn.Messages {
    public class NewVehicleMessage {
        public string Registration { get; set; }
        public string ModelName { get; set; }
        public string ManufacturerName { get; set; }
        public string Color { get; set; }
        public int Year { get; set; }
        public DateTimeOffset ListedAt { get; set; }
        public override string ToString() {
            return $"{Registration} ({ManufacturerName} {ModelName}, {Year}, {Color}) - listed at {ListedAt:O}";
        }

        public NewVehiclePriceMessage AddPrice(int price, string currencyCode) {
            return new NewVehiclePriceMessage() {
                Registration = this.Registration,
                Color = this.Color,
                ListedAt = this.ListedAt,
                ManufacturerName = this.ManufacturerName,
                ModelName = this.ModelName,
                Year = this.Year,
                Price = price,
                CurrencyCode = currencyCode
            };
        }
    }

    public class NewVehiclePriceMessage : NewVehicleMessage {
        public int Price { get; set; }
        public string CurrencyCode { get; set; }
        public override string ToString() => $"{base.ToString()} ({Price} {CurrencyCode}";
    }
}

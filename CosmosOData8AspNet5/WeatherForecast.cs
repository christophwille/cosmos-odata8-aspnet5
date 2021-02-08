using Newtonsoft.Json;
using System;

namespace CosmosOData8AspNet5
{
	public class WeatherForecast
	{
		[JsonProperty(PropertyName = "id")]
		public string Id { get; set; }
		[JsonProperty(PropertyName = "pk")]
		public string Pk { get; set; }

		public DateTime Date { get; set; }

		public int TemperatureC { get; set; }

		[JsonIgnore]
		public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);

		public string Summary { get; set; }
	}
}

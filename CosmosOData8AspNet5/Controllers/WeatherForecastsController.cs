using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CosmosOData8AspNet5.Controllers
{
	public class WeatherForecastsController : ODataController
	{
		private readonly Container _container;
		private readonly ILogger<WeatherForecastsController> _logger;

		public WeatherForecastsController(CosmosClient client, ILogger<WeatherForecastsController> logger)
		{
			_container = client.GetContainer(Startup.CosmosDb, Startup.CosmosContainer);
			_logger = logger;
		}

		[HttpGet]
		[EnableQuery]
		public async Task<IEnumerable<WeatherForecast>> Get()
		{
			// await GenerateDemoData();
			return _container.GetItemLinqQueryable<WeatherForecast>(true);
		}

		private async Task GenerateDemoData()
		{
			const string partitionKey = "wf";
			await _container.CreateItemAsync<WeatherForecast>(new WeatherForecast()
			{
				Id = "1",
				Pk = partitionKey,
				TemperatureC = 10,
				Date = DateTime.UtcNow,
				Summary = "Some data"

			}, new PartitionKey(partitionKey));
		}
	}
}

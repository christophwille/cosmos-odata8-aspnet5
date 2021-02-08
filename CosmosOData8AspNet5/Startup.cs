using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OData.Edm;
using Microsoft.OData.ModelBuilder;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CosmosOData8AspNet5
{
	public class Startup
	{
		public static readonly string CosmosAccountEndpoint = "https://localhost:8081";
		public static readonly string CosmosAccountKey = "{YOUR-KEY-HERE}";
		public static readonly string CosmosDb = "demodb";
		public static readonly string CosmosContainer = "democontainer";

		public Startup(IConfiguration configuration)
		{
			Configuration = configuration;
		}

		public IConfiguration Configuration { get; }

		public void ConfigureServices(IServiceCollection services)
		{
			services.AddControllers();

			services.AddOData(opt => opt.AddModel("odata", GetEdmModel())
				.Filter()
				.Select()
				.Expand());

			services.AddSingleton(sp =>
			{
				return new CosmosClient(CosmosAccountEndpoint, CosmosAccountKey, new CosmosClientOptions()
				{
					SerializerOptions = new CosmosSerializationOptions()
					{
						PropertyNamingPolicy = CosmosPropertyNamingPolicy.CamelCase
					}
				});
			});
		}

		private static IEdmModel GetEdmModel()
		{
			ODataConventionModelBuilder builder = new ODataConventionModelBuilder();
			builder.EntitySet<WeatherForecast>("WeatherForecasts");
			return builder.GetEdmModel();
		}

		public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}

			app.UseHttpsRedirection();

			app.UseRouting();

			app.UseAuthorization();

			app.UseEndpoints(endpoints =>
			{
				endpoints.MapControllers();
			});
		}
	}
}

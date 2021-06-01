using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.OData;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OData.Edm;
using Microsoft.OData.ModelBuilder;

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

			services.AddControllers().AddOData(
				opt => opt.Count().Filter().Expand().Select()
					.AddModel(GetEdmModel()));

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

			services.AddSwaggerGen();
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

			// Use odata route debug, /$odata
			app.UseODataRouteDebug();

			// Add OData /$query middleware
			app.UseODataQueryRequest();

			app.UseSwagger();
			app.UseSwaggerUI(c =>
			{
				c.SwaggerEndpoint("/swagger/v1/swagger.json", "OData 8.x OpenAPI");
			});

			app.UseRouting();

			app.UseAuthorization();

			app.UseEndpoints(endpoints =>
			{
				endpoints.MapControllers();
			});
		}
	}
}

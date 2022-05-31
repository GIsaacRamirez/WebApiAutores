using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using System.Text.Json.Serialization;
using WebApiAutores.Middlewares;

namespace WebApiAutores
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            // Add services to the container.

            services.AddControllers(opciones =>
            {
                // Filtro de manera global
                //opciones.Filters.Add(typeof(FiltroDeExcepcion));
            }).AddJsonOptions(x => x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles)
            ;

            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("defaultConnection"))
                );

            //services.AddTransient<MiFiltroDeAccion>();
            //services.AddHostedService<EscribirEnArchivo>();

            //services.AddResponseCaching();

            //services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer();

            //services.AddTransient<ServicioA>();

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer();
            //services.AddResponseCaching();

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen(c => {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "WebAPIAutores", Version = "v1" });
            });
            services.AddAutoMapper(typeof(Startup));
        }


        /// <summary>
        /// Configurar middleware
        /// </summary>
        /// <param name="app"></param>
        /// <param name="env"></param>
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILogger<Startup> logger)
        {
            // middleware para guardar todas las respuestas
            app.LoguearRespuestaHTTP();

           
            // Configure the HTTP request pipeline.
            if (env.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.UseRouting();

            //app.UseResponseCaching();

            app.UseAuthorization();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });

        }
    }
}

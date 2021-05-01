using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using Persistencia;
using MediatR;
using Aplicacion.AppBooks;
using FluentValidation.AspNetCore;
using WebAPI.Middleware;
using Dominio;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Aplicacion.Contratos;
using Aplicacion.Seguridad;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.OpenApi.Models;

namespace WebAPI
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {


            services.AddCors(

           options => {
               options.AddPolicy("cors", builder =>
               {
                   builder.AllowAnyHeader();
                   builder.AllowAnyMethod();

                   builder.AllowAnyOrigin();
               });
           }

           );

            services.AddControllers();


            /*Lo que agrego*/
            services.AddDbContext<BooksOnlineContext>(
                        opt =>
                        {
                            opt.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"));
                        }

                );
            services.AddMediatR(typeof(Aplicacion.AppBooks.SelectBookID.Manejador).Assembly);
            services.AddControllers(
                // añado policia  para todos los controles osea todos sean por autorizacion
                opt=>
                {
                    var policy = new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build();
                    opt.Filters.Add(new AuthorizeFilter(policy));
                 }
                                
                ).AddFluentValidation(cfg =>
            {
                cfg.RegisterValidatorsFromAssemblyContaining<CreateBook>();
            });

            var builder = services.AddIdentityCore<Usuario>();
            var identityBuilder = new IdentityBuilder(builder.UserType, builder.Services);
            identityBuilder.AddEntityFrameworkStores<BooksOnlineContext>();
            identityBuilder.AddSignInManager<SignInManager<Usuario>>();
            services.TryAddSingleton<ISystemClock, SystemClock>();
            services.AddScoped<IJwtGenerator, JwtGenerator>();

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("Mi palabra secreta"));

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(
                opt => {
                    opt.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey=true,
                        IssuerSigningKey=key,
                        ValidateAudience= false,
                        ValidateIssuer = false
                    };
                }
                );

                services.AddControllersWithViews().AddNewtonsoftJson(option => {
                option.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
                });

            services.AddSwaggerGen(c => {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Services prueba tecnica",
                    Version = "v1"
                });
                c.CustomSchemaIds(c => c.FullName);
            });


        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            /*Lo que añado */

            app.UseCors("cors");

            app.UseMiddleware<ManejadorErrorMiddleware>();

            /*Fin de lo que añado */

            if (env.IsDevelopment())
            {

                /*  Comentareo esta  parte
                app.UseDeveloperExceptionPage();*/
            }


            app.UseAuthentication();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });


            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
                c.RoutePrefix = string.Empty;
            });
            app.UseDefaultFiles();
            app.UseStaticFiles();

        }
    }
}

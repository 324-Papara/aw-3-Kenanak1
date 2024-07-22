using Autofac;
using Autofac.Extensions.DependencyInjection;
using System.Reflection;
using System.Text.Json.Serialization;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using FluentValidation.AspNetCore;
using Microsoft.OpenApi.Models;
using Para.Api.Middleware;
using Para.Api.Service;
using Para.Bussiness;
using Para.Bussiness.Cqrs;
using Para.Data.Context;
using Para.Data.UnitOfWork;
using FluentValidation;

namespace Para.Api
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            // JSON serileþtirme ayarlarý
            services.AddControllers().AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
                options.JsonSerializerOptions.WriteIndented = true;
                options.JsonSerializerOptions.PropertyNamingPolicy = null;
            });

            // Swagger ayarlarý
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Para.Api", Version = "v1" });
            });

            // DbContext ayarlarý
            var connectionStringSql = Configuration.GetConnectionString("MsSqlConnection");
            services.AddDbContext<ParaDbContext>(options => options.UseSqlServer(connectionStringSql));

            // Dependency Injection ayarlarý
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            // AutoMapper ayarlarý
            var mapperConfig = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new MapperConfig());
            });
            services.AddSingleton(mapperConfig.CreateMapper());

            // MediatR ayarlarý
            services.AddMediatR(typeof(CreateCustomerCommand).GetTypeInfo().Assembly);

            // FluentValidation ayarlarý
            services.AddFluentValidationAutoValidation()
                .AddValidatorsFromAssemblyContaining<CustomerValidator>()
                .AddValidatorsFromAssemblyContaining<CustomerAddressValidator>()
                .AddValidatorsFromAssemblyContaining<CustomerDetailValidator>()
                .AddValidatorsFromAssemblyContaining<CustomerPhoneValidator>();

            // Middleware'ler
            services.AddScoped<RequestResponseLoggingMiddleware>();

            // Custom servisler
            services.AddTransient<CustomService1>();
            services.AddScoped<CustomService2>();
            services.AddSingleton<CustomService3>();

            // Register Autofac
            services.AddAutofac();
        }

        public void ConfigureContainer(ContainerBuilder builder)
        {

            builder.RegisterAssemblyModules(Assembly.GetExecutingAssembly());

 
            builder.Register(c => new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<MapperConfig>();
            })).AsSelf().SingleInstance();
            builder.Register(ctx => ctx.Resolve<MapperConfiguration>().CreateMapper())
                .As<IMapper>()
                .SingleInstance();

            builder.RegisterAssemblyTypes(typeof(CreateCustomerCommand).Assembly)
                .AsImplementedInterfaces()
                .InstancePerLifetimeScope();


            builder.RegisterAssemblyTypes(typeof(CustomerValidator).Assembly)
                .Where(t => t.IsSubclassOf(typeof(AbstractValidator<>)))
                .AsSelf()
                .InstancePerLifetimeScope();


            builder.RegisterType<CustomService1>().AsSelf().InstancePerLifetimeScope();
            builder.RegisterType<CustomService2>().AsSelf().InstancePerLifetimeScope();
            builder.RegisterType<CustomService3>().AsSelf().SingleInstance();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Para.Api v1"));
            }

            // Middleware'ler
            app.UseMiddleware<HeartbeatMiddleware>();
            app.UseMiddleware<ErrorHandlerMiddleware>();
            app.UseMiddleware<RequestResponseLoggingMiddleware>();

            // HTTPS yönlendirmesi
            app.UseHttpsRedirection();

            // Routing ve Authorization
            app.UseRouting();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            // Custom request logging
            app.Use(async (context, next) =>
            {
                if (!string.IsNullOrEmpty(context.Request.Path) && context.Request.Path.Value.Contains("favicon"))
                {
                    await next();
                    return;
                }

                var service1 = context.RequestServices.GetRequiredService<CustomService1>();
                var service2 = context.RequestServices.GetRequiredService<CustomService2>();
                var service3 = context.RequestServices.GetRequiredService<CustomService3>();

                service1.Counter++;
                service2.Counter++;
                service3.Counter++;

                await next();
            });

            app.Run(async context =>
            {
                var service1 = context.RequestServices.GetRequiredService<CustomService1>();
                var service2 = context.RequestServices.GetRequiredService<CustomService2>();
                var service3 = context.RequestServices.GetRequiredService<CustomService3>();

                if (!string.IsNullOrEmpty(context.Request.Path) && !context.Request.Path.Value.Contains("favicon"))
                {
                    service1.Counter++;
                    service2.Counter++;
                    service3.Counter++;
                }

                await context.Response.WriteAsync($"Service1 : {service1.Counter}\n");
                await context.Response.WriteAsync($"Service2 : {service2.Counter}\n");
                await context.Response.WriteAsync($"Service3 : {service3.Counter}\n");
            });
        }
    }
}

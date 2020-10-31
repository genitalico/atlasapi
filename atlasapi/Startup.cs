using System;
using atlasapi.mongodb;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace atlasapi
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
            var cs = Environment.GetEnvironmentVariable("MONGO_CS");
            var dbName = Environment.GetEnvironmentVariable("MONGO_DBNAME");
            var collection1 = Environment.GetEnvironmentVariable("MONGO_COLLECTION");
            var dbPassword = Environment.GetEnvironmentVariable("MONGO_PASSWORD");
            var dbUser = Environment.GetEnvironmentVariable("MONGO_USER");
            var sizeCode = Convert.ToInt32(Configuration["SizeCode"].ToString());

            var split = cs.Split("//");

            cs = split[0] + "//" + dbUser + ":" + dbPassword + "@" + split[1] + "/" + dbName + "?authSource=admin";

            services.AddSingleton<IMongoTransaction, MongoTransaction>(x =>
            {
                return new MongoTransaction(cs,dbName,collection1);
            });

            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
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

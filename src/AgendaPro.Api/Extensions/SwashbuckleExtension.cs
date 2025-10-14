namespace AgendaPro.Api.Extensions
{
    public static class SwashbuckleExtension
    {
        public static void AddApplicationSwagger(this IServiceCollection services)
        {
            services.AddSwaggerGen();
        }


        public static void UseApplicationSwagger(this WebApplication app)
        {
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger(e =>
                {
                    e.OpenApiVersion = Microsoft.OpenApi.OpenApiSpecVersion.OpenApi2_0;
                });
                app.UseSwaggerUI(e =>
                {
                });
            }
        }
    }
}

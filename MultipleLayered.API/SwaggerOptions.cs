namespace Multiple_Layered.API
{
    public static class SwaggerOptions
    {
        public static void ConfigureSwagger(this IServiceCollection services)
        {
            services.AddSwaggerGen(setup =>
            {
                var jwtSecurityScheme = new OpenApiSecurityScheme
                {
                    BearerFormat = "JWT",
                    Name = "API Kimlik Doğrulama",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.Http,
                    Scheme = JwtBearerDefaults.AuthenticationScheme,
                    Description = "Aldığınız Token'ı Girin",

                    Reference = new OpenApiReference
                    {
                        Id = JwtBearerDefaults.AuthenticationScheme,
                        Type = ReferenceType.SecurityScheme
                    },

                };

                var openApiInfo = new OpenApiInfo
                {
                    Title = "Multiple Layered API",
                    Version = "V1",
                    Description = @"Multiple-Layered.API.v1.0.0 Için Yapılan Ilk API.<br> 
                                    Debug İşleminden Sonra Bu Ekranı Görüyorsanız Console Log Kaydını Kontrol Ediniz.<br> 
                                    Kullanıcılar, Roller, Claimler için Birer Örnek Data Yüklenecektir.<br> 
                                    Giriş Bilgileri; <br>
                                    <strong> Email: admin@example.com <br>
                                    Şifre: Melissa.34 </strong> <br>",

                };

                setup.SwaggerDoc("v1", openApiInfo);

                setup.AddSecurityDefinition(jwtSecurityScheme.Reference.Id, jwtSecurityScheme);

                setup.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    { jwtSecurityScheme, Array.Empty<string>() }
                });
            });
        }
    }
}

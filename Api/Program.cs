bool dev = true;


WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Services.AddDefaultAWSOptions(builder.Configuration.GetAWSOptions());
builder.Services.AddAWSService<IAmazonS3>();
builder.Services.Configure<FormOptions>(o => {
    o.ValueLengthLimit = int.MaxValue;
    o.MultipartBodyLengthLimit = int.MaxValue;
    o.MemoryBufferThreshold = int.MaxValue;
});

builder.Services.AddControllers()
    .AddJsonOptions(options => options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);

/**
 * Swagger configuration
 */
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    OpenApiSecurityScheme jwtSecurityScheme = new OpenApiSecurityScheme()
    {
        Scheme = "bearer",
        BearerFormat = "JWT",
        Name = "JWT authentication",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Description = "JWT Bearer :",

        Reference = new OpenApiReference()
        {
            Id = JwtBearerDefaults.AuthenticationScheme,
            Type = ReferenceType.SecurityScheme
        },
    };

    options.AddSecurityDefinition(jwtSecurityScheme.Reference.Id, jwtSecurityScheme);

    options.AddSecurityRequirement(new OpenApiSecurityRequirement()
    {
        {
            jwtSecurityScheme,
            Array.Empty<string>()
        }
    });
});

/**
 * JWT configuration
 */
builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters()
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration.GetSection("AppSettings:Token:Key").Value)),
            ValidateIssuer = false,
            ValidateAudience = false,
        };
    });

/**
 * Handlers container singleton
 */
builder.Services.AddDbContext<ModelsContext>(options => options
    .UseNpgsql(builder.Configuration.GetConnectionString("MusicOverflowHeroku"),
        optionBuilder =>
        {
            optionBuilder.MigrationsAssembly("Api");
            optionBuilder.UseQuerySplittingBehavior(QuerySplittingBehavior.SplitQuery);
        }),
        contextLifetime: ServiceLifetime.Transient);

try
{
    builder.Services.AddSingleton<HandlersContainer>();
}
catch (HandlerNotFoundException exception)
{
    Console.WriteLine(exception.Message);
    return;
}

/**
 * Launch app
 */
builder.Services.AddCors();

WebApplication app = builder.Build();

if (dev)
{
    AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
}

app.UseCors(policy => policy
    .AllowAnyOrigin()
    .AllowAnyHeader()
    .AllowAnyMethod()
);

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();

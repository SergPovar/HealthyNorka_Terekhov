using System.Reflection;
using System.Text.Json.Serialization;
using HealthyNorka_Terekhov;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    options.JsonSerializerOptions.Converters.Add(new DateTimeJsonConverter());
});
   
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
 builder.Services.AddSwaggerGen(c => {

     // Set the comments path for the Swagger JSON and UI.
     var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
     var xmlPath = Path.Combine(AppContext.BaseDirectory,xmlFile);
     c.IncludeXmlComments(xmlPath);
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
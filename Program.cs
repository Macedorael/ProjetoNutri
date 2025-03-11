
using Microsoft.EntityFrameworkCore;
using ProjetoNutri.Context;


var builder = WebApplication.CreateBuilder(args);


// Add services to the container.
builder.Services.AddDbContext<ClienteContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("ConexaoPadrao")));

// builder.Services.AddDbContext<FocoContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("ConexaoPadrao")));



builder.Services.AddControllersWithViews();
// Registro do serviço CalculosDobras
builder.Services.AddScoped<ProjetoNutri.Services.CalculosDobras>();
builder.Services.AddScoped<ProjetoNutri.Services.CalculosCircunferencia>();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();
app.Use(async (context, next) =>
{
    if (context.Request.Path == "/")
    {
        context.Response.Redirect("/Paciente");
        return;
    }
    await next();
});

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Paciente}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();

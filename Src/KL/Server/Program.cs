using KL.Server.Resources;
using KL.Server.Settings;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// builder.Services.AddControllersWithViews();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAuthentication().AddCookie(options =>
{
    options.Cookie.HttpOnly = true;
    options.Cookie.SameSite = SameSiteMode.Strict;
    options.Events.OnRedirectToLogin = context =>
    {
        if (context.Request.Path.StartsWithSegments("/api"))
        {
            context.RedirectUri = new PathString("/api");
            context.Response.StatusCode = StatusCodes.Status404NotFound;
        }
        else
        {
            context.RedirectUri = new PathString("/");
        }
        return Task.CompletedTask;
    };
});

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("RequireInternalUserRole",
        policy => policy.RequireRole("InternalUser"));
});

builder.Services.AddRazorPages();

builder.Services.Configure<CosmosSettings>(
    builder.Configuration.GetSection(nameof(CosmosSettings)));

builder.Services.AddSingleton<ResourceRepository>();
builder.Services.AddScoped<IResourceService, ResourceService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseWebAssemblyDebugging();
}
else
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();



app.UseBlazorFrameworkFiles();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();


app.UseEndpoints(endpoint =>
{
    endpoint.MapRazorPages();
    endpoint.MapControllers();
    endpoint.Map("api/{**slug}", context =>
    {
        context.Response.StatusCode = StatusCodes.Status404NotFound;
        return Task.CompletedTask;
    } );
   
    endpoint.MapFallbackToFile("{**slug}", "index.html");
});

app.Run();
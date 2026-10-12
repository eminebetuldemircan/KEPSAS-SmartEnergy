using SmartEnergy.Web.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

var apiBase = new Uri("http://localhost:5165/");

builder.Services.AddHttpClient<DashboardApiService>(client => { client.BaseAddress = apiBase; });
builder.Services.AddHttpClient<MeterApiService>(client => { client.BaseAddress = apiBase; });
builder.Services.AddHttpClient<ConsumptionRecordApiService>(client => { client.BaseAddress = apiBase; });
builder.Services.AddHttpClient<AuthApiService>(client => { client.BaseAddress = apiBase; });
builder.Services.AddHttpClient<SubscriptionApiService>(client => { client.BaseAddress = apiBase; });
builder.Services.AddHttpClient<BillApiService>(client => { client.BaseAddress = apiBase; });
builder.Services.AddHttpClient<NotificationApiService>(client => { client.BaseAddress = apiBase; });
builder.Services.AddHttpClient<PaymentApiService>(client => { client.BaseAddress = apiBase; });
builder.Services.AddHttpClient<ApplicationApiService>(client => { client.BaseAddress = apiBase; });

builder.Services.AddDistributedMemoryCache();

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(60);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
    options.Cookie.Name = ".KepsasPortal.Session";
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseStaticFiles();

app.UseRouting();

app.UseSession();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Account}/{action=Login}/{id?}");

app.Run();

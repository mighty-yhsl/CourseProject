using CourseProject.BLL.Dependency_Injection;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllersWithViews();
// custom dependency injection
builder.Services.AddDependencies();
var app = builder.Build();


if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Transport}/{action=GetAllTransports}/{id?}");

app.Run();

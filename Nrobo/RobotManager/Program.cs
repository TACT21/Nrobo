namespace RobotManager
{
    public class Program
    {
        public static void Main(string[] args)
        {

            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddSignalR();

            var app = builder.Build();

            app.MapHub<ControlManager>("/");

            try
            {
                ControlManager.Initializer();
                app.MapGet("/Status", () => "All right.");
            }
            catch (Exception ex)
            {
                app.MapGet("/Status", () => ex.ToString());
            }

            app.Run();
        }
    }
}
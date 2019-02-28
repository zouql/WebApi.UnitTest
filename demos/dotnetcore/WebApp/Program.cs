namespace WebApp
{
    using System;
    using System.IO;
    using Microsoft.AspNetCore;
    using Microsoft.AspNetCore.Hosting;

    public class Program
    {
        static Program()
        {
            InitDir();
        }

        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>();

        private static void InitDir()
        {
            var binDir = $"{Environment.CurrentDirectory}/bin";

            Directory.CreateDirectory(binDir);
        }
    }
}

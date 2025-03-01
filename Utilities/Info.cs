namespace LearnAPI.Utilities
{
    public class Info
    {
       // public static string DbConnection = @"Data Source=DESKTOP-US2RJ9L;Initial Catalog=TheBookNook;Integrated Security=True;Trust Server Certificate=True";

        public static string DbConnection { get; private set; }

        static Info()
        {
            var config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();

            DbConnection = config.GetConnectionString("DefaultDb");
        }
    }
}

namespace Migration.DbContexts
{
    public static class DbConfigurations
    {
        public static string ScContext { get; set; } = "Server=localhost;Port=5432;User Id=postgres;Password=!QAZ2wsx#EDC;Database=NacidSc";
        public static string RndContext { get; set; } = "Data Source=DESKTOP-CLS26AJ;Initial Catalog=NacidRnd;User ID=sa;Password=!QAZ2wsx#EDC; TrustServerCertificate=True";
	}
}

namespace Migration.DbContexts.Rnd.Models
{
    public class ComplexBasicPart
    {
        public int Id { get; set; }

        public int EntityId { get; set; }
        public ComplexBasic Entity { get; set; }

        public int? State { get; set; }
    }
}

namespace Sc.Models.Entities.Base
{
    public interface IEntityVersion : IEntity
    {
        int Version { get; set; }
        int ViewOrder { get; set; }
    }
}

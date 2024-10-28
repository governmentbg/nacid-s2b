namespace Sc.Models.Entities.Base
{
    public abstract class EntityVersion : Entity, IEntityVersion
    {
        public int Version { get; set; }
        public int ViewOrder { get; set; }
    }
}

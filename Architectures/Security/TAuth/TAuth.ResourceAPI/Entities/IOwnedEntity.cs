namespace TAuth.ResourceAPI.Entities
{
    public interface IOwnedEntity
    {
        public int OwnerId { get; set; }
    }
}

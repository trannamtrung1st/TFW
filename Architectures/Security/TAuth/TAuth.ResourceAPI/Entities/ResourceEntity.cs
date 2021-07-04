namespace TAuth.ResourceAPI.Entities
{
    public class ResourceEntity
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int UserId { get; set; }

        public virtual AppUser User { get; set; }
    }
}

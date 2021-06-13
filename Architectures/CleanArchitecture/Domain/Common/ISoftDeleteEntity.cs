namespace Domain.Common
{
    public interface ISoftDeleteEntity
    {
        bool Deleted { get; set; }
    }
}

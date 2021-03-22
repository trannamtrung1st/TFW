namespace TFW.Cross.Entities
{
    public class TagOfPost
    {
        public string TagLabel { get; set; }
        public int PostId { get; set; }

        public virtual Tag Tag { get; set; }
        public virtual Post Post { get; set; }
    }
}

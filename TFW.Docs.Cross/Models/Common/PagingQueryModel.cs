namespace TFW.Docs.Cross.Models.Common
{
    public abstract class PagingQueryModel
    {
        public int Page { get; set; } = QueryConsts.DefaultPage;
        public int PageLimit { get; set; } = QueryConsts.DefaultPageLimit;
    }
}

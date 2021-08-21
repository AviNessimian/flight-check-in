namespace Britannica.Application.Bases
{
    public abstract class PaginatedRequest
    {
        public int PageIndex { get; protected set; }
        public int TotalPages { get; protected set; }
    }
}

namespace Britannica.Host.Filters
{
    public class UpdateConcurrencyProblemDetails
    {
        public string Type { get; set; }
        public string Message { get; set; }

        public string EntityType { get; set; }
        public string Description { get; set; }
    }
}

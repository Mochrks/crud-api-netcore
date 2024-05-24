
namespace WebApi.DTO.Request
{
    public class ProductFilterRequestDTO
    {
        public string? ProductName { get; set; }

        public string? SortBy { get; set; }
        public int? PageSize { get; set; }
        public int? PageNumber { get; set; }
    }
    public enum SortDirection
    {
        Ascending,
        Descending
    }
}


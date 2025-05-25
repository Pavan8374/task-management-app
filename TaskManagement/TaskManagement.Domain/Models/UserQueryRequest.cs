namespace TaskManagement.Domain.Models
{
    public class UserQueryRequest
    {
        public int Page { get; set; }
        public int PageSize { get; set; }
        public string Search { get; set; }
        public string SortBy { get; set; }
        public bool IsAsc { get; set; }
        public int UserId { get; set; }
        public bool IsActive { get; set; }
    }
}

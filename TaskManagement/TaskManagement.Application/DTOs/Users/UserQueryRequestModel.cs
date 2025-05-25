namespace TaskManagement.Application.DTOs.Users
{
    public class UserQueryRequestModel
    {
        public int Page { get; set; }
        public int PageSize { get; set; }
        public string Search { get; set; }
        public string SortBy { get; set; }
        public bool IsAsc { get; set; }
    }
}

public class PaginatedTodoTask<T>
{
    public List<T> Tasks { get; set; } = new List<T>() { };
    public int TotalCount { get; set; }
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
    public int TotalPages => (int)Math.Ceiling((double)TotalCount / PageSize);

}

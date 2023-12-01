namespace BlazorApp1.Shared
{
    public class PostDTO
    {
        public string? Id { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string? Author { get; set; }
    }
}

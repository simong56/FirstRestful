namespace BlogApi.Models.DTOs;

public class AddNewBlogpostDto
{
    public string? Title { get; set; }
    public string? Content { get; set; }
    public int BloggerId { get; set; }
}
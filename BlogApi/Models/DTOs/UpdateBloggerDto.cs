namespace BlogApi.Models.DTOs;

public class UpdateBloggerDto
{
    public int IdToUpdate { get; set; }
    public string? Name { get; set; }
    public string? Email { get; set; }
    public int? Age { get; set; }
    public string? Password { get; set; }
}
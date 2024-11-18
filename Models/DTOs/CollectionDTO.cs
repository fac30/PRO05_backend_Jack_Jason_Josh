namespace J3.Models.DTOs;

public class CreateCollectionDTO
{
    public string? Name { get; set; }
    public string? Description { get; set; }
    public string? Type { get; set; }
    public bool IsPublic { get; set; }
    public int? UserId { get; set; }
} 
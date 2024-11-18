namespace J3.Models.DTOs;

public class CreateCollectionDTO
{
    public required string Name { get; set; }
    public string? Description { get; set; }
    public required string Type { get; set; }
    public required bool IsPublic { get; set; }
    public required int UserId { get; set; }
} 
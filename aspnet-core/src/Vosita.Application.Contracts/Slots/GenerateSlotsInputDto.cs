using System.ComponentModel.DataAnnotations;

public class GenerateSlotsInputDto
{
    [Required]
    [RegularExpression(@"^\d{4}-\d{2}-\d{2}$", ErrorMessage = "Use YYYY-MM-DD")]
    public string StartDate { get; set; } = default!;
    
    [Required]
    [RegularExpression(@"^\d{4}-\d{2}-\d{2}$", ErrorMessage = "Use YYYY-MM-DD")]
    public string EndDate { get; set; } = default!;
    
    [Required]
    public string TimeZone { get; set; } = default!;
    
    [Required]
    [Range(1, 1440, ErrorMessage = "Duration must be between 1 and 1440 minutes")]  // 1440 for the whole day 
    public int SlotDuration { get; set; }
}
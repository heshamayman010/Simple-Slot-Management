namespace Vosita.Slots;

public class GetNextAvailableSlotsInputDto
{
    public string TimeZone { get; set; } = "UTC";
    public int Count { get; set; } = 20;
}
using System.ComponentModel.DataAnnotations;

namespace DiscordBotTemplate.Models.Logs;

public class ErrorLog
{
    [Key]
    public int key { get; set; }
    public DateTime errorTime { get; set; }
    public string? location { get; set; }
    public string? message { get; set; }
}

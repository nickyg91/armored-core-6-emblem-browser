namespace ArmoredCoreSixEmblemBrowser.Entities;

public class Emblem
{
    public int Id { get; set; }
    public string ShareId { get; set; }
    public PlatformType Platform { get; set; }
    public string Name { get; set; }
    public string? FileName { get; set; }
}
namespace PluginFramework.Core.Images.DTOs;

public class ApplyEffectsDto
{
    public string ImageBase64 { get; set; }
    public Dictionary<string, List<string>> ChoosenEffects { get; set; }
}

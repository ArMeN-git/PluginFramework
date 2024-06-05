namespace PluginFramework.PluginBase;

public interface IImageManipulator
{
    string PluginName { get; }
    List<string> EffectNames { get; }
    Task<string> ApplyEffects(string imageBase64, List<string> effectNames);
}

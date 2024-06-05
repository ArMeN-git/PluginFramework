using PluginFramework.PluginBase;

namespace PluginFramework.AdobePlugin;

internal class AdobeImageManipulator : IImageManipulator
{
    public string PluginName => "Adobe";

    public List<string> EffectNames => ["Resize image"];

    public Task<string> ApplyEffects(string imageBase64, List<string> effectNames)
    {
        var bytes = Convert.FromBase64String(imageBase64);

        // demonstration of possible manipulation operation
        // simulating asynchronous operation
        // for simplicity, assuming effectNames are given back the exact same way as UI receives from plugins

        var effectsToApply = EffectNames.Intersect(effectNames);
        var newBase64 = Convert.ToBase64String(bytes);

        return Task.FromResult(newBase64);
    }
}

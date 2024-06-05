using PluginFramework.Core.Images.DTOs;
using PluginFramework.PluginBase;
using System.Reflection;

namespace PluginFramework.Core.Images;

internal class ImageManager : IImageManager
{
    public List<PluginDto> GetAvailablePlugins(List<string> desiredPlugins)
    {
        var result = new List<PluginDto>();
        var implementingTypes = LoadImplementingTypes(typeof(IImageManipulator),
                            Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), "..", "Plugins")));
        var pluginNames = implementingTypes.Keys.Select(v => v.Split('.')[0]).ToList();

        var missingPlugins = desiredPlugins.Except(pluginNames);
        if (missingPlugins.Any())
        {
            foreach (var plugin in missingPlugins)
            {
                Console.WriteLine($"Missing plugin {plugin}");
            }
        }

        var finalPlugins = pluginNames.Intersect(desiredPlugins);
        foreach (var plugin in finalPlugins)
        {
            var type = implementingTypes[plugin + ".dll"];
            PropertyInfo pluginNameProperty = type.GetProperty("PluginName");
            PropertyInfo effectNamesProperty = type.GetProperty("EffectNames");

            object instance = Activator.CreateInstance(type);
            var pluginName = (string)pluginNameProperty.GetValue(instance);
            var effectNames = (List<string>)effectNamesProperty.GetValue(instance);

            result.Add(new PluginDto
            {
                PluginName = pluginName,
                ProvidedEffects = effectNames
            });
        }

        return result;
    }

    public Task<string[]> ApplyEffectsToImages(List<ApplyEffectsDto> imagesWithEffects)
    {
        var implementingTypes = LoadImplementingTypes(typeof(IImageManipulator),
                            Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), "..", "Plugins")));

        var tasks = new List<Task<string>>();
        var pluginNames = implementingTypes.Keys.Select(v => v.Split('.')[0]).ToList();

        foreach (var imageModel in imagesWithEffects)
        {
            var imagebase64 = imageModel.ImageBase64;
            foreach (var plugin in imageModel.ChoosenEffects)
            {
                var effects = plugin.Value;
                // get plugin from types
                // call Apply method by reflection, by passing imageBase64 and effects parameters
                // add task to tasks List
            }
        }
        // lookup for selected plugin types, call methods by reflection
        // by passing effects as parameters

        return Task.WhenAll(tasks);
    }

    private static Dictionary<string, Type> LoadImplementingTypes(Type interfaceType, string path)
    {
        var implementingTypes = new Dictionary<string, Type>();
        DirectoryInfo directoryInfo = new(path);
        FileInfo[] files = directoryInfo.GetFiles("*.dll");

        foreach (var file in files)
        {
            try
            {
                Assembly assembly = Assembly.LoadFrom(file.FullName);
                Type[] types = assembly.GetTypes();
                foreach (Type type in types)
                {
                    if (interfaceType.IsAssignableFrom(type) && !type.IsInterface && !type.IsAbstract)
                    {
                        string[] parts = file.Name.Split('.');
                        string lastPart = parts[^2] + "." + parts[^1];
                        implementingTypes[lastPart] = type;
                        break;
                    }
                }
            }
            catch (ReflectionTypeLoadException ex)
            {
                Console.WriteLine($"Error loading types from {file.Name}: {ex.Message}");
            }
        }

        return implementingTypes;
    }
}

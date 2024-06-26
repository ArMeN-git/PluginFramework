﻿using PluginFramework.Core.Images.DTOs;

namespace PluginFramework.Core.Images;

public interface IImageManager
{
    List<PluginDto> GetAvailablePlugins(List<string> desiredPlugins);
    Task<string[]> ApplyEffectsToImages(List<ApplyEffectsDto> imagesWithEffects);
}

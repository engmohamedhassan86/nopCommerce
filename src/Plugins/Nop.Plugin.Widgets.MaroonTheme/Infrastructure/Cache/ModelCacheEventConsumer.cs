using Nop.Core.Caching;
using Nop.Core.Domain.Configuration;
using Nop.Core.Events;
using Nop.Services.Events;

namespace Nop.Plugin.Widgets.MaroonTheme.Infrastructure.Cache;

/// <summary>
/// Model cache event consumer (used to invalidate the cached style block when settings change)
/// </summary>
public class ModelCacheEventConsumer :
    IConsumer<EntityInsertedEvent<Setting>>,
    IConsumer<EntityUpdatedEvent<Setting>>,
    IConsumer<EntityDeletedEvent<Setting>>
{
    #region Fields

    private readonly IStaticCacheManager _staticCacheManager;

    #endregion

    #region Ctor

    public ModelCacheEventConsumer(IStaticCacheManager staticCacheManager)
    {
        _staticCacheManager = staticCacheManager;
    }

    #endregion

    #region Methods

    /// <summary>
    /// Handle entity inserted event
    /// </summary>
    /// <param name="eventMessage">Event message</param>
    /// <returns>A task that represents the asynchronous operation</returns>
    public async Task HandleEventAsync(EntityInsertedEvent<Setting> eventMessage)
    {
        await _staticCacheManager.RemoveByPrefixAsync(MaroonThemeDefaults.StyleBlockPrefix);
    }

    /// <summary>
    /// Handle entity updated event
    /// </summary>
    /// <param name="eventMessage">Event message</param>
    /// <returns>A task that represents the asynchronous operation</returns>
    public async Task HandleEventAsync(EntityUpdatedEvent<Setting> eventMessage)
    {
        await _staticCacheManager.RemoveByPrefixAsync(MaroonThemeDefaults.StyleBlockPrefix);
    }

    /// <summary>
    /// Handle entity deleted event
    /// </summary>
    /// <param name="eventMessage">Event message</param>
    /// <returns>A task that represents the asynchronous operation</returns>
    public async Task HandleEventAsync(EntityDeletedEvent<Setting> eventMessage)
    {
        await _staticCacheManager.RemoveByPrefixAsync(MaroonThemeDefaults.StyleBlockPrefix);
    }

    #endregion
}

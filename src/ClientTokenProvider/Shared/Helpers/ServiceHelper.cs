﻿namespace ClientTokenProvider.Shared.Helpers;

internal static class ServiceHelper
{
    public static TService? GetService<TService>()
        => Current.GetService<TService>();

    public static TService GetRequiredService<TService>()
        where TService : notnull
        => Current.GetRequiredService<TService>();

    public static IServiceProvider Current
        =>
#if WINDOWS10_0_17763_0_OR_GREATER
        MauiWinUIApplication.Current.Services;
#elif ANDROID
        IPlatformApplication.Current!.Services;
#elif IOS || MACCATALYST
        IPlatformApplication.Current!.Services;
#else
        null;
#endif
}

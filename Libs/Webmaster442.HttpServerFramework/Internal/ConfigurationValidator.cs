// ------------------------------------------------------------------------------------------------
// Copyright (c) 2021-2022 Ruzsinszki Gábor
// This is free software under the terms of the MIT License. https://opensource.org/licenses/MIT
// -----------------------------------------------------------------------------------------------

namespace Webmaster442.HttpServerFramework.Internal;

internal static class ConfigurationValidator
{
    public static void ValidateAndTrhowExceptions(HttpServerConfiguration configuration)
    {
        if (configuration.MaxClients < 1)
            throw new InvalidOperationException($"{nameof(configuration.MaxClients)} clients should be at least 1");

        if (configuration.MaxPostSize < 0)
            throw new InvalidOperationException($"{nameof(configuration.MaxPostSize)} must be non negative");
    }
}

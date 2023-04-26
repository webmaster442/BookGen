// ------------------------------------------------------------------------------------------------
// Copyright (c) 2023 Ruzsinszki Gábor
// This is free software under the terms of the MIT License. https://opensource.org/licenses/MIT
// -----------------------------------------------------------------------------------------------

using System.Diagnostics.CodeAnalysis;
using System.Net;

namespace Webmaster442.HttpServerFramework;

/// <summary>
/// Allows comparing IP adresses for equality
/// </summary>
public class IPAdressComparer : IEqualityComparer<IPAddress>
{
    /// <inheritdoc/>
    public bool Equals(IPAddress? x, IPAddress? y)
    {
        return x?.AddressFamily == y?.AddressFamily
            && Compare(x?.GetAddressBytes(), y?.GetAddressBytes());
    }

    private static bool Compare(byte[]? bytes1, byte[]? bytes2)
    {
        if (bytes1 == null && bytes2 == null) return true;
        if (bytes1 == null || bytes2 == null) return false;

        if (bytes1.Length != bytes2.Length)
            return false;

        for (int i=0; i<bytes1?.Length; i++)
        {
            if (bytes1[i] != bytes2[i])
            {
                return false;
            }
        }
        return true;
    }

    /// <inheritdoc/>
    public int GetHashCode([DisallowNull] IPAddress obj)
    {
        HashCode hash = new HashCode();
        hash.Add(obj.AddressFamily);
        var bytes = obj.GetAddressBytes();
        for (int i=0; i<bytes.Length; i++)
        {
            hash.Add(bytes[i]);
        }
        return hash.ToHashCode();
    }
}

//-----------------------------------------------------------------------------
// (c) 2022 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Update.Infrastructure;
using System.Security.Cryptography;

namespace BookGen.Update.Steps;

internal sealed class VerifyHash : IUpdateStepAsync
{
    public string StatusMessage => "Verifying update...";

    public async Task<bool> Execute(GlobalState state)
    {
        if (!File.Exists(state.TempFile))
        {
            state.Issues.Add("Downloaded update file not found");
            return false;
        }

        using (var hash = SHA256.Create())
        {
            if (hash == null)
            {
                state.Issues.Add("Hash compute error");
                return false;
            }

            using (FileStream file = File.OpenRead(state.TempFile))
            {
                byte[] hashBytes = await hash.ComputeHashAsync(file);
                string computed = BitConverter.ToString(hashBytes).Replace("-", "");
                if (string.Compare(computed, state.Latest.HashSha256, true) == 0)
                {
                    return true;
                }
            }
        }

        state.Issues.Add("Downloaded Package integrity check failed");
        return false;
    }
}

﻿using BookGen.Update.Dto;
using BookGen.Update.Infrastructure;
using System.Text.Json;

namespace BookGen.Update.Steps;

internal sealed class DownloadReleaseInfo : IUpdateStepAsync
{
    private const string UpdateUrl = "https://raw.githubusercontent.com/webmaster442/BookGen/master/.github/updates.json";

    public async Task<bool> Execute(GlobalState state)
    {
        using (var client = new HttpClient())
        {
            using (HttpResponseMessage? response = await client.GetAsync(UpdateUrl))
            {
                if (response.IsSuccessStatusCode)
                {
                    string? content = await response.Content.ReadAsStringAsync();
                    if (!string.IsNullOrEmpty(content))
                    {
                        Release[]? result = JsonSerializer.Deserialize<Release[]>(content);
                        if (result != null)
                        {
                            state.Releases = result;
                            return true;
                        }
                    }
                }

                state.Issues.Add($"Ar error occured during downloading of update.json: {response}");
                return false;
            }
        }
    }
}

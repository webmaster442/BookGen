﻿//-----------------------------------------------------------------------------
// (c) 2023 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using Spectre.Console;
using System.Diagnostics;
using System.Reflection;

namespace BookGen.Gui;

public abstract class MenuBase : IDisposable
{
    private CancellationTokenSource? _tokenSource;

    protected abstract Task OnRender(Renderer renderer);

    private static string Localize(string text)
    {
        if (text == null)
            ArgumentNullException.ThrowIfNull(text);

        string? translated = Properties.Resources.ResourceManager.GetString(text);
        return translated ?? text;
    }

    protected static Dictionary<string, T> GetEnumItems<T>() where T : struct, Enum
    {
        T[] values = Enum.GetValues<T>();
        var items = new Dictionary<string, T>(values.Length);
        Type t = typeof(T);
        foreach (T value in values)
        {
            FieldInfo? fieldInfo = t.GetField(value.ToString());
            TextAttribute? textAttribute = fieldInfo?.GetCustomAttribute<TextAttribute>();

            if (textAttribute?.Id != null)
            {
                items.Add(Localize(textAttribute.Id), value);
            }
            else
            {
                throw new InvalidOperationException($"{value} doesn't have name");
            }
        }
        return items;
    }

    public async Task Run()
    {
        if (_tokenSource != null)
            throw new InvalidOperationException("Allready running");

        _tokenSource = new CancellationTokenSource();

        var renderer = new Renderer(AnsiConsole.Console,
                                    Properties.Resources.ResourceManager,
                                    _tokenSource.Token);

        try
        {
            Console.CancelKeyPress += OnCancel;
            await OnRender(renderer);
        }
        catch (TaskCanceledException)
        {
            Debug.WriteLine("Run() Task cancelled");
        }
        finally
        {
            Console.CancelKeyPress -= OnCancel;
        }
    }

    private void OnCancel(object? sender, ConsoleCancelEventArgs e)
    {
        _tokenSource?.Cancel();
        e.Cancel = true;
    }

    public void Cancel()
    {
        _tokenSource?.Cancel();
    }

    protected virtual void Dispose(bool disposing)
    {
        if (_tokenSource != null)
        {
            _tokenSource.Dispose();
            _tokenSource = null;
        }
    }

    public void Dispose()
    {
        Dispose(false);
        GC.SuppressFinalize(this);
    }
}

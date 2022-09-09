﻿namespace BookGen.Launcher.Contracts.Activation;

public interface IActivationHandler
{
    bool CanHandle();

    Task HandleAsync();
}

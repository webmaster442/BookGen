﻿using BookGen.Domain.TaskRunner;

namespace BookGen.DomainServices;

public class BookGenTaskFactory
{
    public static BookGenTasks CreateSample()
    {
        return new BookGenTasks
        {
            Tasks = new BookGenTask[]
            {
                new BookGenTask
                {
                    Name = "Git pull",
                    Items = CreateSingleCommand("git pull"),
                },
                new BookGenTask
                {
                    Name = "Commit & Push",
                    Items = new TaskItem[]
                    {
                        new InputPrompt
                        {
                            Message = "Commit message",
                            Varialbe = "gitcommitmsg",
                        },
                        new ShellCommands
                        {
                            ShellType = ShellType.Powershell,
                            Commands = "git add .\r\n"+
                                   "git commit -m \"$Env:gitcommitmsg\"\r\n"+
                                   "git push"
                        }
                    }
                }
            }
        };
    }

    private static TaskItem[] CreateSingleCommand(string cmd)
    {
        return new TaskItem[]
        {
            new ShellCommands
            {
                Commands = cmd,
                ConditionVariable = string.Empty,
                ShellType = ShellType.Powershell
            }
        };
    }
}

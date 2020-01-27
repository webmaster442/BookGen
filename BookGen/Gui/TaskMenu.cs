//-----------------------------------------------------------------------------
// (c) 2019-2020 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Api;
using BookGen.Core;
using BookGen.Domain.VsTasks;
using BookGen.Gui.Elements;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BookGen.Gui
{
    internal class TaskMenu : ConsoleMenuBase
    {
        private readonly ILog _log;
        private readonly FsPath _tasksFile;
        private readonly bool _isWindows;
        private bool _readkey;

        public TaskMenu(ILog log, FsPath WorkDir)
        {
            _log = log;
            _tasksFile = WorkDir.Combine(".vscode\\tasks.json");
            _isWindows = Environment.OSVersion.Platform == PlatformID.Win32NT;
            _readkey = true;
        }

        private void StartProcess(string cmd, string args)
        {
            try
            {
                using (var p = new System.Diagnostics.Process())
                {
                    p.StartInfo.FileName = cmd;
                    p.StartInfo.Arguments = args;
                    p.StartInfo.UseShellExecute = false;
                    p.Start();
                }
            }
            catch (Exception ex) when 
                (ex is InvalidOperationException
                || ex is System.ComponentModel.Win32Exception)
            {
                _log.Warning(ex);
            }
        }

        private void RunTask(Task? task)
        {
            if (task == null)
                return;

            string command = "";
            string args = "";
            if (_isWindows)
            {
                if (task?.type == "shell")
                {
                    command = "cmd.exe";
                    args = $"/c {task?.windows?.command} {task?.windows?.args?.FirstOrDefault()?.value}";
                }
                else
                {
                    command = task?.windows?.command ?? "";
                    args = task?.windows?.args?.FirstOrDefault()?.value ?? "";
                }
            }
            else
            {
                command = task?.command ?? "";
                args = task?.args?.FirstOrDefault()?.value ?? "";
            }
            StartProcess(command, args);
        }

        private Button CreateBackPageButton()
        {
            return new Button
            {
                Action = () =>
                {
                    ShouldRun = false;
                    _readkey = false;
                },
                Content = "Back to previous menu"
            };
        }

        private Button CreateButtonFromTask(Task task)
        {
            return new Button
            {
                Content = task?.label ?? "",
                Action = () => RunTask(task)
            };
        }

        public override IEnumerable<ConsoleUiElement> CreateElements()
        {
            VsTaskRoot? tasks = null;
            if (_tasksFile.IsExisting)
            {
                tasks = _tasksFile.DeserializeJson<VsTaskRoot>(_log);
            }

            yield return new TextBlock
            {
                Content = "------------------------------------------------\r\n"
                        + " Tasks\r\n"
                        + " .vscode\\tasks.json\r\n"
                        + "------------------------------------------------\r\n\r\n"
            };

            if (tasks == null
                || tasks.tasks == null 
                || tasks.tasks.Count < 1)
            {
                yield return new TextBlock
                {
                    Content = "No Tasks file found. Use Initializer to create one\r\n\r\n"
                };
                yield return CreateBackPageButton();
            }
            else
            {
                yield return new TextBlock
                {
                    Content = "Tasks:\r\n"
                };
                foreach (var task in tasks.tasks)
                {
                    yield return CreateButtonFromTask(task);
                }
                yield return new TextBlock
                {
                    Content = "\r\nNavigation:\r\n"
                };
                yield return CreateBackPageButton();
            }
        }

        protected override void ProcessInputs()
        {
            int? index = Renderer.GetInputChoice();

            Button? actionToDo = null;

            if (index.HasValue)
            {
                actionToDo = (from item in Elements
                              where
                                 item is Button b
                                 && b.Entry == index.Value
                              select
                                 item as Button).FirstOrDefault();
            }
            if (actionToDo == null)
            {
                DoRender();
                Renderer.DisplayError($"\rUnrecognised item: {index}\r\n");

            }
            else
            {
                actionToDo.Action?.Invoke();
                if (_readkey)
                {
                    Renderer.PressKeyContinue();
                }
                else
                {
                    _readkey = true;
                }
                DoRender();
            }
        }
    }
}

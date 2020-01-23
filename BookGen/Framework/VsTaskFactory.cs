//-----------------------------------------------------------------------------
// (c) 2019 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using BookGen.Domain.ArgumentParsing;
using BookGen.Domain.VsTasks;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;

namespace BookGen.Framework
{
    internal static class VsTaskFactory
    {
        private static Task CreateTask(string arguments, string winArgs, string Description)
        {
            return new Task
            {
                type = "shell",
                command = "BookGen",
                args = new List<Arg>
                {
                    new Arg
                    {
                        quoting = Quoting.escape,
                        value = arguments,
                    },
                },
                windows = new Windows
                {
                    command = "BookGen",
                    args = new List<Arg>
                    {
                        new Arg
                        {
                            quoting = Quoting.escape,
                            value = winArgs,
                        },
                    },
                },
                group = "build",
                label = Description,
                presentation = new Presentation
                {
                    clear = true,
                    echo = true,
                    focus = false,
                    group = "build",
                    panel = Panel.shared,
                    reveal = Reveal.always
                },
            };
        }

        private static string DescriptionAttr<T>(this T source) where T: struct
        {
            string name = source.ToString() ?? "";
            FieldInfo? fi = source.GetType().GetField(name);

            if (fi == null)
                return string.Empty;

            DescriptionAttribute[] attributes = (DescriptionAttribute[])fi.GetCustomAttributes(
                typeof(DescriptionAttribute), false);

            if (attributes?.Length > 0)
                return attributes[0].Description;
            else 
                return name;
        }

        internal static VsTaskRoot CreateTasks()
        {
            return new VsTaskRoot
            {
                version = "2.0.0",
                tasks = CreateTaskItems().ToList()
            };
        }

        private static IEnumerable<Task> CreateTaskItems()
        {
            var tasks = Enum.GetNames(typeof(ActionType));
            foreach (var task in tasks)
            {
                ActionType value = (ActionType)Enum.Parse(typeof(ActionType), task);
                yield return CreateTask($"Build -a {value} -d $PWD",
                                         $"Build -a {value} -d %cd%", 
                                         value.DescriptionAttr());

            }
        }
    }
}

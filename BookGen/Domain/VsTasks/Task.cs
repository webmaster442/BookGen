//-----------------------------------------------------------------------------
// (c) 2020 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

using System.Collections.Generic;

namespace BookGen.Domain.VsTasks
{
    public class Task
    {
#pragma warning disable IDE1006 // Naming Styles
        /// <summary>
        ///  The task's label used in the user interface.
        /// </summary>
        public string? label { get; set; }
        /// <summary>
        /// The task's type. For a custom task, this can either be shell or process.
        /// If shell is specified, the command is interpreted as a shell command 
        /// (for example: bash, cmd, or PowerShell). 
        /// If process is specified, the command is interpreted as a process to execute.
        /// </summary>
        public string? type { get; set; }
        /// <summary>
        /// The actual command to execute.
        /// </summary>
        public string? command { get; set; }
        /// <summary>
        /// Arguments
        /// </summary>
        public List<Arg>? args { get; set; }
        /// <summary>
        /// Any Windows specific properties. Will be used instead of the default properties when the command is executed on the Windows operating system.
        /// </summary>
        public Windows? windows { get; set; }
        /// <summary>
        /// Defines to which group the task belongs. 
        /// </summary>
        public string? group { get; set; }
        /// <summary>
        /// Defines how the task output is handled in the user interface.
        /// </summary>
        public Presentation? presentation { get; set; }
#pragma warning restore IDE1006 // Naming Styles
    }
}

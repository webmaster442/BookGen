//-----------------------------------------------------------------------------
// (c) 2020 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

namespace BookGen.Domain.VsTasks
{
#pragma warning disable IDE1006 // Naming Styles
    public enum Reveal
    {
        /// <summary>
        ///  The panel is always brought to front. This is the default.
        /// </summary>
        always,
        /// <summary>
        /// The user must explicitly bring the terminal panel to the front using the View > Terminal command (Ctrl+`).
        /// </summary>
        never,
        /// <summary>
        /// The terminal panel is brought to front only if the output is not scanned for errors and warnings.
        /// </summary>
        silent
    }

    public enum Panel
    {
        /// <summary>
        /// The terminal is shared and the output of other task runs are added to the same terminal.
        /// </summary>
        shared,
        /// <summary>
        /// The terminal is dedicated to a specific task. If that task is executed again, the terminal is reused. However, the output of a different task is presented in a different terminal.
        /// </summary>
        dedicated,
        /// <summary>
        ///  Every execution of that task is using a new clean terminal.
        /// </summary>
        @new
    }

    public class Presentation
    {
        /// <summary>
        /// Controls whether the Integrated Terminal panel is brought to front. 
        /// </summary>
        public Reveal? reveal { get; set; }
        /// <summary>
        /// Controls whether the terminal is taking input focus or not. Default is false.
        /// </summary>
        public bool? focus { get; set; }
        /// <summary>
        /// Controls whether the executed command is echoed in the terminal. Default is true.
        /// </summary>
        public bool? echo { get; set; }
        /// <summary>
        /// Controls whether the terminal instance is shared between task runs.
        /// </summary>
        public Panel panel { get; set;}
        /// <summary>
        /// Controls whether the terminal is cleared before this task is run. Default is false.
        /// </summary>
        public bool? clear { get; set; }
        /// <summary>
        /// Controls whether the task is executed in a specific terminal group using split panes. Tasks in the same group (specified by a string value) will use split terminals to present instead of a new terminal panel.
        /// </summary>
        public string? group { get; set; }

    }
}
#pragma warning restore IDE1006 // Naming Styles
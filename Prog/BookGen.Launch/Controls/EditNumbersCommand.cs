//-----------------------------------------------------------------------------
// (c) 2021 Ruzsinszki Gábor
// This code is licensed under MIT license (see LICENSE for details)
//-----------------------------------------------------------------------------

namespace BookGen.Launch.Controls
{
    internal class EditNumbersCommand : EditCommand
    {
        public EditNumbersCommand(MarkdownEditor editor, bool wrapLeft, bool wrapRight) : base(editor, wrapLeft, wrapRight)
        {
        }

        public override void Execute(object? parameter)
        {
            base.Execute(parameter);
        }
    }
}
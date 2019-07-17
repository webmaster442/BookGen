// Copyright (c) Microsoft Corporation.  All rights reserved.

namespace ColorCode.Compilation
{
    public static class RuleFormats
    {
        private const string script = @"(?xs)(<)(script)
                                        {0}[\s\n]+({1})[\s\n]*(=)[\s\n]*(""{2}""){0}[\s\n]*(>)
                                        (.*?)
                                        (</)(script)(>)";

        private const string attributes = @"(?:[\s\n]+([a-z0-9-_]+)[\s\n]*(=)[\s\n]*([^\s\n""']+?)
                                           |[\s\n]+([a-z0-9-_]+)[\s\n]*(=)[\s\n]*(""[^\n]+?"")
                                           |[\s\n]+([a-z0-9-_]+)[\s\n]*(=)[\s\n]*('[^\n]+?')
                                           |[\s\n]+([a-z0-9-_]+) )*";

        public static readonly string JavaScript = string.Format(script, attributes, "type|language", "[^\n]*javascript");

        public static readonly string ServerScript = string.Format(script, attributes, "runat", "server");
    }
}
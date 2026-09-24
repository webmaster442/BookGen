# Scripting

Scripting in BookGen lets you automate common workflows for your book - building outputs, generating or transforming content.
You can run scripts from your shell (PowerShell, Bash, etc.) that invoke BookGen commands, or you can define and run scripts
directly inside BookGen.

The advantage of scripting inside BookGen is performance: BookGen starts once and executes multiple script steps in the
same process, avoiding repeated startup overhead. Use shell scripts when you need OS integration or CI ties;
use BookGen scripts when you want faster, BookGen-focused automation.

## Execution model

BookGen scripts follow a bash `pipefail` style execution model. Commands run sequentially, and the script continues only
as long as each command succeeds. If any single command fails, execution stops immediately and the whole script is
considered failed - the remaining commands are not run. This fail-fast behavior ensures that a broken step never lets the
rest of the script proceed on invalid state, making script results reliable and easy to reason about.

When a script fails, its exit code is the exit code of the last command that was executed - that is, the command that
failed and caused execution to stop. This lets you inspect the script's exit code from your shell or CI pipeline to
determine exactly which step went wrong and to react accordingly.

## Syntax

Beside the execution model described above, BookGen scripts follow the usual bash conventions.
Long arguments that contain spaces must be escaped by wrapping them in quotes. For example:

```
build --output "./my output folder"
```

Without the quotes the value would be split into multiple arguments and the command would not receive the intended input.

Each line should contain only a single command together with its arguments. For readability, however, a command can be
split across multiple lines by ending a line with the `\` character. When a line ends with `\`, the following line is
treated as a continuation of the same command. For example:

```
build \
    --output "./my output folder" \
    --verbose
```

The example above is equivalent to writing the whole `build` command on one line.

## Comments

BookGen scripts support two comment styles, and only these two:

- Shell-style comments that begin with a `#` mark.
- C++-style line comments that begin with `//`.

Everything from the comment marker to the end of the line is ignored during execution. Both styles are line comments only -
there is no block or multi-line comment syntax. You can use either style (or mix them) to document your scripts.

```
# This is a comment
build --output ./out   # trailing comment after a command

// This is also a comment
clean                  // trailing comment using C++ style
```

### Adding extra logging

In addition to regular comments, BookGen scripts let you inject your own messages into the log output while a script is
running. To emit a custom log message, start a line with the special `#log` or `//log` instruction, followed by the text
you want to display:

```
#log Starting the build step
build --output ./out

//log Build finished, cleaning up
clean
```

These log instructions must appear at the very start of the line. If `#log` or `//log` is not at the beginning of the line
(for example when used as a trailing comment after a command), it is treated as an ordinary comment and its text is ignored
during execution instead of being written to the log:

```
build --output ./out   #log this is NOT logged, it's just a regular trailing comment
```

Use these log instructions to make your scripts easier to follow, marking progress and highlighting important steps in the
runtime output.

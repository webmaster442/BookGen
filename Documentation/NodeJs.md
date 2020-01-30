# NodeJs Integration

BookGen can be integrated to work with NodeJs scripts. You can write HTML outputing extensions that will be parsed as Shortcodes by BookGen in Javascript uising Node.js.

To make this feature work you have to have an installed Node.Js in a directory that can be found on the Path (eg. NodeJs can be started from any shell window).

Then you can use the following shortcode to run your Node script: `<!--{NodeJs script.js}-->

For Security and interop compatibility options the maximum time that a NodeJs can use to execute is limited in 60 seconds.
# ILog Interface

Namespace: BookGen.Api

 Interface for logging. Provides methods that allow information logging to console. 

## Methods

* `Void Log( BookGen.Api.LogLevel logLevel,  System.String format,  System.Object[] args);`
     Log a message 
    * `logLevel`: Log level
    * `format`: Message, a fomat string that can be handled by the String.Format method
    * `args`: Arguments for formatting
* `Void Critical( System.String format,  System.Object[] args);`
     Log a Critical error. Critcal error is an error that causes the program to stop working 
    * `format`: Message, a fomat string that can be handled by the String.Format method
    * `args`: Arguments for formatting
* `Void Critical( System.Exception ex);`
     Log a critical exception. 
    * `ex`: Exception to log
* `Void Warning( System.String format,  System.Object[] args);`
     Log a warning message. Warning is an error that shouldn't happen idealy, but we are expecting it to occure. 
    * `format`: Message, a fomat string that can be handled by the String.Format method
    * `args`: Arguments for formatting
* `Void Warning( System.Exception ex);`
     Log a warning exception. 
    * `ex`: Exception to log
* `Void Info( System.String format,  System.Object[] args);`
     Log an iformational message. Informations give the user feedback about what is happening. 
    * `format`: Message, a fomat string that can be handled by the String.Format method
    * `args`: Arguments for formatting
* `Void Detail( System.String format,  System.Object[] args);`
     Log a detail. Details are usually not important, so Details are only displayed when verbose output is enabled. 
    * `format`: Message, a fomat string that can be handled by the String.Format method
    * `args`: Arguments for formatting

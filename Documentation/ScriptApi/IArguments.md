# IArguments Interface

Namespace: BookGen.Api

 Represents Arguments and values that are passed to the Script. Argument names are case insensitive. 

## Properties

* `IEnumerable<String> ArgumentNames { get;  }`
     Argument names -> all lowercase 

* `String Item { get;  }`
    

## Methods

* `Boolean HasArgument( System.String name);`
     Checks if the specified argument name is in the collection or not. 
    * `name`: Argument name to search
* `T GetArgumentOrFallback( System.String argument,  T fallback);`
     Tries to get argument value and convert it to type. If argument not found fallback value will be returned 
    * `argument`: Argument to get
    * `fallback`: Fallback value if argument not found
* `T GetArgumentOrThrow( System.String argument);`
     Get argument value and convert it to type. If argument not found an exception will be thrown 
    * `argument`: Argument to get

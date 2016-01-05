Welcome to Gents function jumper, a derivative of RawCode's weird assembly stuff...

Obsolete:

-Function Overriding using string

-Ability to directly use MethodInfo

Done:

-Event System 

-Allow the passing of return type of a function to avoid an AmbiguousMatchException for functions with overrides.

ToDo:

-Some sort of method to auto duplicate the Event class for every function. (Possibly Generics)

-Allow the passing of return type of a function to avoid an AmbiguousMatchException for functions with overrides. 

-Check that OnCalledGate() and the function to override have the same return type and parameters. IN PROGRESS!

-Add exception catches.

~~-When multiple Functions are added to the same event, only the last one added gets to passback its return value. Implement the passing back of an LIST of return values~~

-Support Out keyword
# SimpleCalculator
A simple calculator allows users do simple operations: add, subtract and multiply values in set of registers. Registers are also supported as values with lazy evaluation.
The program can handle circular dependencies. Please, check examples to understand the approach behind it. 

## Run

The program is developed on .NET 6, C#. 
Users can run the program on Windows via [dotnet commands](https://learn.microsoft.com/en-us/dotnet/core/tools/dotnet).
If .NET SDK or Runtimes are not installed on Windows, it is necessary to install them before. 
Useful links:
[.NET installation](https://learn.microsoft.com/en-us/dotnet/core/install/windows?tabs=net70). Users can choose any runtime or sdk, because all of them support Console apps. 
[dotnet installation](https://learn.microsoft.com/en-us/dotnet/core/tools/dotnet-install-script).

When previous steps are done, it is possible to run the application. Please follow those steps:

1. Download [project](https://www.dropbox.com/scl/fo/25u6ntel5uwqa4ocd6hqb/h?rlkey=9xol9p7co344u5mwta3kaftn9&dl=0).
2. In powershell open the folder containing SimpleCalculator.sln.
3. Run command bellow to build the project. The sucessful output should contain message "Build succeeded. 0 Warning(s) 0 Error(s)".
```
dotnet build
```
4. Move to SimpleCalculator folder where SimpleCalculator.csproj is stored.
```
cd .\SimpleCalculator\
```
5. Run the program.
```
dotnet run "filePath"
```

filePath is case insensitive.

The program has 2 modes: "Console" and "File". 
"File" mode is used when file is provided as an argument. If users enter more than 1 argument, only the 1st will be used. If file is not found, the program will be opened in "Console" mode.  
"Console" mode is used when file is not provided as an argument or not found.

## Commands

Performs a given operation with given value on given register. 
```
<register> <operation> <value>
```

Evaluates and prints the value of the register. Numbers will be considered as a name of register. If register is not setup, it will returns 0.
```
print <register>
```

Exits the program. 
```
quit
```

Where:
* Register: Any alphanumerical input. Allowed symbols are A-Z, a-z, 0-9.
* Operation: add, substract, or multiply.
* Value: Either a integer or a register. Int can be from -2,147,483,648 to 2,147,483,647, other numbers will be evaludated as register name.

During calculations results can be in the interval -9,223,372,036,854,775,808 to 9,223,372,036,854,775,807, however, if they exceed those values, result will be upredictable.
All input is case insensitive.

## Examples

Example 1. Insensitive case. Operations order.
```
cost add iphone15
IPHONE15 add 12999
IPHONE15 multiply 2
iphone15Pro add 14999
iphone15Pro multiply 2
cost add iphone15Pro
print cost
IphoneExchange add 1000
cost subtract iphoneExchange
print cost
QUIT
```
Output
```
55996
54996
```

Example 2. "Print" lazy evaluates value "a" based on commands written before the "print". Next commands are taken into account during the next print command.
The first "print a" check b, b equals 0 therefore "print a" return 0. Then "b" becomes 5 after "add" and "multiply", but it doesn't affect "print a" and it returns 0 again.
Only after "a add b" "a" becomes 5 and "print a" returns 5.
```
a add b
print a
b add 1
b multiply 5
print a
a add b
print a
```
Output
```
0
0
5
```

Example 3. Circular dependencies are allowed. Start with "b add a", "a" is unknown, so "a" evaluation is required. The next command is "a add b", "a" depends on "b" (circular dependencies),
so the program goes back to "b" evaluation. The next "b" command is "b add c", "c" is unknown, so "c" evaluation is required. The next command is "c add b", "c" depends on "b" 
(circular dependencies), so the program goes back to "b" evaluation. The next "b" command is "b add 3", therefore "b" becomes 3. Previous "c add b" makes "c" equuals 3 and 
"b add c" makes "b" 6. The program evaluates "a add b", "a" equals 6 as well. The next "a" command is "a add 5" that makes "a" equals 11. After calculating "a", "b add a" can be
evaluated and after this "b" is 17.

```
a add b
b add a
c add b
b add c
a add 5
b add 3
print b
```
Output
```
17
```

Example 4. Invalid Operation. Command was not processed. Program continues to work.
```
a added b
print a
```
Output
```
0
```



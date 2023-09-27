# SimpleCalculator
A simple calculator allows users do simple operations: add, subtract and multiply values in set of registers. Registers are also supported as values with lazy evaluation.
Circular dependencies are not allowed. 

## Run

```
./SimpleCalculator.exe "file"
```

File is case insensitive.
The program has 2 modes: "Console" and "File". 
"File" mode is used when file is provided as an argument. If users enter more than 1 argument, only the 1st will be used. If file is not found, the program will be opened in "Console" mode.  
"Console" mode is used when file is not provided as an argument or not found.

## Commands

Performs a given operation with given value on given register. 
```
<register> <operation> <value>
```

Evaluates and prints the value of the register. Numbers will be considered as a name of register. If register was not setup, it will returns 0.
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
* Value: Either a long or a register. Long can be from -9,223,372,036,854,775,808 to 9,223,372,036,854,775,807, other numbers will be evaludated as register name.

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

Example 3. Circular dependencies are not allowed. The program returns Error.
```
a add b
b add a
print a

```
Output
```
Error
```

Example 4. Invalid Operation. Command was not processed.
```
a added b
print a

```
Output
```
Error
```



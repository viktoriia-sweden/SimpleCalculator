# SimpleCalculator
A simple calculator allows users do simple operations: add, subtract and multiply values in set of registers. Registers are also supported as values with lazy evaluation. Circular dependencies
are not allowed. 

## Run

```
./SimpleCalculator.exe "file"
```

File is case insensitive.
The program has 2 modes: "Console" and "File". 
"File" mode is used when file is provided as an argument. If users enter more than 1 argument, only the 1st will be used. If file is not found, the program will be opened in "Console" mode.  
"Console" mode is used when file is not provided as an argument or not found.

## Syntax

* Register: Any alphanumerical input. Allowed symbols are A-Z, a-z, 0-9.
* Operation: add, substract, or multiply.
* Value: Either a long or a register.

All input is case insensitive.

### Operation
Performs a given operation with given value on given register. 
```
<register> <operation> <value>
```

### Print
Evaluates and prints the value of the register. Numbers will be considered as a name of register. If register was not setup, it will returns 0 and warning message.
```
print <register>
```

### Quit
Exits the program.
```
quit
```

## What you can do

Input
```
cost add iphone15
cost add iphone15Pro
IPHONE15 add 12999
IPHONE15 multiply 2
iphone15Pro add 14999
iphone15Pro multiply 2
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

---

The calculator can handle circular dependencies.

Input
```
A add B
B add C
C add A
A add 3
print A
print B
print C
quit
```
Output
```
3
3
3
```

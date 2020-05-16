# Description

Milan is a mini language described in book.

Source code is represented by single file. No multiple source files, packages/modules etc.

Functions aren't supported.

Negative numbers in source code aren't supported, but you can write 0 - 123.

## Grammar

**bold** are terminals.

Rule | Description
-----|-------------
P -> **begin** L **end**                                    | Program starts with **begin** keyword and ends with **end**
L -> S _or_ S **;**                                         | List of statements. If there are more than statement then they are separeted with semicolon
S -> **id** **:=** E                                        | Assigment statement
S -> **while** B **do** L **od**                            | While loop
S -> **if** B **then** L **fi**                             | if condition
S -> **if** B **then** L **else** L **fi**                  | if condition with else branch
S -> **write** **(** E **)**                                | Writes  value expression to stdout.
B -> E **rel** E                                            | Comparison expression. **rel** can be one of **=**,**<>**, **<**, **<=**, **>**, **>=**
E -> E **ots** T _or_ T                                     | Expression. **ots** can be one of **+**,**-**
T -> T **otm** P _or_ P                                     | Term (if i get it right) **otm** can be one of **\***, **\/**,**\%**
P -> **id** _or_ **const** _or_ **(** E **)** _or_ **read** | Production(if i get it right)

**id** is an identifier. Starts with character.

**const** is a constant. Only signed integers are supported. No floats, strings, etc.

**read** reads number from user input.

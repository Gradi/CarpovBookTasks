# Dummy virtual machine

Dummy virtual machine (dvm) is a simple stack vm described in book with some
additional instructions to control stack location and size.

VM uses 32-bit addresses(pointers) and 32-bit signed integers and doesn't support any other datatypes (e.g. strings, floats, etc).
It's stack can only contain 32-bit signed integers.

Op codes have 8-bit unsigned size.

All numbers are little-endian.

## Instructions

Mnemonic    | Code | Description                                    | Sample
----------- | ---- | ---------------------------------------------- | ------
HLT         | 0x00                          | Halts exectuion of vm | HLT
JMP `m`     | 0x01 \<pointer>               | Unconditional jump to address `m` | JMP 0x000001AE
JMP0 `m`    | 0x02 \<pointer>               | Jumps to `m` if top stack element is `0`. Pops element from stack | JMP0 0x0000FFA0
JMP1 `m`    | 0x03 \<pointer>               | Jumps to `m` if top stack element is `not zero`. Pops element from stack  | JMP1 0x0000AAEE
INN         | 0x04                          | Reads word from stdin and pushes it to the stack | INN
PRN         | 0x05                          | Pops word from stack and prints it to the stdout | PRN
LD `m`      | 0x06 \<pointer>               | Loads word at the address `m` and pushes it to the stack | LD 0x00000002
ST `m`      | 0x07 \<pointer>               | Pops word from the stack and stores it at the address `m` | ST 0x00000002
ADD         | 0x08                          | Pops two words from stack, sums them up and result is pushed to the stack | ADD
SUB         | 0x09                          | Pops two words, second word is subtracted from the first and result is pushed to the stack | SUB
MUL         | 0x0A                          | Pops two words, multiples them and result is pushed to the stack | MUL
DIV         | 0x0B                          | Pops two words, first word is divided by second and result is pushed to the stack | DIV
MOD         | 0x0C                          | Pops two words, first word is modulo divided by second and result is pushed to the stack | MOD
CMP `i`     | 0x0D `i`                      | Pops two words, performs comparison and result `0`(false) or `1`(true) is pushed to the stack. `i` defines type of comparison and has 8-bit unsigned size. `i=0` for `equality`, `i=1` for `not equal`, `i=2` for `less than`, `i=3` for `greater than`, `i=4` for `less or equal than`, `i=5` for `greater or equal than`. | CMP 0x5
STK `m` `a` | 0x0E \<pointer> \<word-count> | Sets stack location and size. Operands are 32-bit signed integers (however values can't be negative). | STK 0x00000F00 0x0000000F

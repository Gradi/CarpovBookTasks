# Description

This repository contains tasks from book "Ю.Г. Карпов -- Основы построения трансляторов"
(Not all of them cause i'm lazy :).

## PrinterNumsLangParser

Project contains parser for a range of numbers like `1,2,4-7,9`.

## RomanNumbersParser

Contains parser for roman numbers. I'm not sure about it's correctness.

## StateMachinesBuilder

Contains helper class `SMachine` to simplify state machine creation.

## GrammarLib

Some classes to create grammars and perform different actions on it. Not complete.

## Milan

Library for parsing Mini language described in book. [Read](Milan/README.md) it's readme for details.

## DummyVirtualMachine

Dummy virtual machine described in book. [Read](DummyVirtualMachine/README.md) it's readme for details.

## milc

Milan compiler. Wrapper around Milan project that compiles source code into dummy machine's code.

## dvm

Wrapper around DummyVirtualMachine that loads and executes binaries produced by milc. It has some methods
to debug running application.

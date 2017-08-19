# What is Ela

Ela is a free (both noncommercial and open source) functional programming language. Ela runs on .NET and Mono and supports Windows, Linux, Mac OS and many other environments. Ela can run in both 32-bit and 64-bit modes. Ela programs can be distributed and executed either as source code files or as binary object files. Ela fully supports interactive mode (REPL) and is shipped with a graphical development environment (Elide).

## Ela is a functional language

You've probably heard this multiple times; there are plenty of languages that claim to be hybryd and to provide full support for functional programming. However, Ela is quite different from them. Unlike many other languages, Ela doesn't make any trade-offs, trying to combine distinct programming paradigms that don't fit well together. And it makes Ela a more powerful and flexible language than any of these hybrids, which is perfectly logical if you think about it. Why would you need to extend a modern car with a steam engine? Or why would you need to extend a functional language with object orientation in mainstream style? If you want to argue with that, first give Ela a try.

## Ela is a dynamic language

If the first thing that comes to your head, when somebody says "dynamic language", is JavaScript, than think twice. Dynamic typing in Ela is lambda calculus and metaprogramming over types, not the "let's scrap all types and formal operation semantics" from many popular dynamic languages. Ela offers you much more static control than most of other dynamic languages. Ela is a type safe, strictly typed language, which is, however, not burden by limitations of any particular type checker.

## Ela is one of a kind

Ela is the first and the only pure functional programming language that combines dynamic typing, algebraic types and type classes and, at the same time, features support for both strict and lazy evaluation strategies. Comparing it to big functional languages such as Haskell, Ela is relatively simple and has a very low entry cost - it can be even used as a scripting language of your choice or be embedded in .NET applications.

# Why Ela

Ela can be used to study and teach functional programming, for prototyping, for writing theorem provers, for scripting, as well as for development of applications in a pure functional way. Ela comes with a rich standard library, interactive console and a graphical development environment. Ela also offers a flexible and powerful interface to .NET programming languages, such as C#.

# Ela features

## Functions, first class

    concat [] = [] //Recursive function
    concat (x::xs) = x ++ concat xs

    concat = foldr (++) [] //The same, through foldr

    //The same function defined using lambda
    concat = \xs -> if isnil xs then [] 
        else head xs ++ concat (tail xs)

In Ela the concept of first class functions is taken to a whole new level. Functions are not just first class, thus rendering some nice programming techniques and capabilities. Functions are the main and the most basic building block in Ela programs, functions are used as a primary tool for abstractions, new functions are defined by partially applying existing functions, even operators in Ela are just regular functions. And all these things are presented through a well thought and elegant syntax, with extensive support for pattern matching, laziness and recursive definitions.

## Modules, also first class

    open list //opening a standard module
    
    rec = { x = 42 } //a record
    {x} = rec //pattern matching record
    
    {concat,concatMap} = list //pattern matching module
    _ = id list //passing module as a function argument

Ela features an expressive and flexible module system. Ela is a unit based compilation language, so that each module can be compiled separately. Compiled modules (.elaobj files) has fast indexed metadata tables which enables high performance run-time reflection. Also modules provides a support for namespacing. Last, but not least, modules in Ela are first class objects just like functions - they be passed as arguments to functions, you can write generic function that can operate with modules and with records at the same time, you can even pattern match modules just like records.

## Algebraic types

    //Algebraic type
    type Option = None | Some a
      deriving Eq Ord Show 
  
    xs = map Some [1..10] //Constructor is a function
    x = head xs //x is Some 1
    (Some y) = x //Pattern match x
    x == None //Returns false

Algebraic types have a set of really useful properties - because of their nature, a lot of standard operations (such as equality, comparisons, formatting to strings, iteration, etc.), which you would normally define manually over and over again, can be inferred automatically by a compiler or by a run-time environment. That saves you a "couple" of key strokes. Also Ela allows you to create open algebraic types which can be extended with new cases at any moment.

## Type classes

    //Class and instance declaration
    class Pointed a where
      point _->a //Overloaded by return type

    instance Pointed List where
      point x = [x] //Instance for a linked list
  
    point 42 ::: List //Outputs: [42]

Types classes in Ela, inspired by Haskell, along with dynamic dispatch and support for function (and constant) overloading by return type, provide a powerful abstraction mechanism. Classes are like interfaces in object oriented languages but they don't unnecessarily tie up functions and values together and are, in fact, considerably more flexible. You can provide your own overloading rules per every function without the need to stick with "dispatch by first argument" rule adoped by OOP. You can even define instances of classes for already existing types, including standard types, such as integers and floats. And in many cases class instances can be inferred for you automatically. In fact most of standard functions in Ela, including arithmetic functions, equality operators, comparison functions and so forth are defined through classes Additive, Ring, Field, Eq, Ord, etc.

## Lazy when you need it
  
    //Infinite list (implicit laziness)
    a = 1 :: a
    
    //Lazy map, explicit laziness
    map' _ [] = []
    map' f (x::xs) = & f x :: map' f xs
    take 10 <| map' (*2) a

Ela provides an extensive support for both strict and non-strict evaluation. Moreover, while preferring eager evaluation, Ela compiler analyzes your program to understand whether it should be executed in a strict or in a lazy manner, and postpones evaluation of certain expressions if this is needed. You can also explicitly mark sections of code as lazy using (&) operator, which creates a thunk (similar to futures from Alice ML). Thanks to this all the programming techniques from lazy functional languages are possible in Ela, while still maintaining predictable and mostly strict program behavior. Not even saying that all bindings in Ela are mutually recursive and their order is generally insignificant (like it should be in a declarative language).

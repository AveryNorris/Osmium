# Solid Objects

### Code and Documentation by Avery Norris

---

Solid objects solve an important problem that arrives when you try to load **Assemblies Dynamically**.

Osmium uses the **.NET** **Type** class to manage and organize external assemblies. However,
this class has one small downfall. If we dynamically load the **same Assembly** twice, the Types between
them will not match. For example

Imagine this Assembly

    Assembly A

        Class Osmium_Rules

And let's imagine we load it twice, with a pretend number at the end so we can tell them apart

    Assembly (0)

        Class Osmium_Rules (0)

    Assembly (1)

        Class Osmium_Rules (1)

Even though it is the same code that creates Osmium_Rules, they will not be treated as equal
by C# reflection

    typeof(Osmium_Rules (0)).Equals(typeof(Osmium_Rules (1))) == false

This alone does not kill the Type, we can use **Type.FullName** to see if they match.
(Which includes AssemblyName as well, so as long as AssemblyNames aren't duplicate with alternative
definitions, this works)

The real problem spans from when we try to reload external assemblies. If you have read
the documentation of **Context**, you will know that Osmium unloads and reloads all existing
external assemblies when packages or code is updated.

This first unloading step **requires** that no references are carried from any of the types
or objects of the assemblies being unloaded. It just so happens that a **Type** counts as a reference.
And therefore will halt the unloading process unless if it exists at unload time.

However, Types are an important identifier when the map is reloaded. Without **Types** we cannot
know what classes to instantiate as Components, or if Client-Defined Scenes or Components
exist. 

The solution to this problem comes in the **Solid Type**. A **Reload-Safe** Type that can be recovered
after the unload. 

## Backend

---

The **Solid Type** is theoretically simple, it records the full name of a type, and it defines a method that
searches all the currently loaded Assemblies for a true C# Type variable. Which can then
be discarded before the next Unload.

There are many other Solid reflection types, which can be used to store information on the
structure of code through a reload.

They also have error correction systems, since code is being unloaded and reloaded; there is no 
guarantee it is the same between loadings (and most times it is not!) so we must make sure
that the structure has not changed, and any SolidTypes or references truly exist in this new loaded
Assembly.
        
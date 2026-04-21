# Osmium Base Class

When you are interacting with the **OsmiumNucleus** you must use the base class **Osmium**. Which provides all the following

    Stores the Static OpenTK Context

    Manages All of the Scenes

    Provides a basic Interface to Interact with Scenes.


 - **Storing The OpenTK Context** is an extremely important job; and it goes to the Osmium class because it is static. To Access it you can simply use Osmium.Context

 
 - **Managing The Scenes** is the main job assigned to the class. It holds all the Scenes within a HashSet and they are identified by their names.


 - Osmium also provides an **Interface for Scenes** to allow you to easily manipulate them using Add, Contains, Get and Remove! 


## Usage

In order to use **Osmium** you must put either

    using OsmiumNucleus;

    or

    using static OsmiumNucleus.Osmium;

At the top of your file!

When you are ready to call **Osmium** it is quite simple. Run **Osmium.Initialize()** to Prepare Osmium To Run Your Game You should add some Components
and Scenes which will run your Game. And then run **Osmium.Run()** to begin the Game Loop!

Once you are ready to exit simply call **Osmium.Close()**! Happy Coding!

---

---
# End Of Documentation
### Code and Documentation by Avery Norris

<p align="center">
  <img src="Osmium.png" width="150px" style="image-rendering: pixelated; align-content: center;" alt="OsmiumLogo">
</p>
# Scene

---

In **Osmium**, The **Scene** is responsible for Containing all the "Action". It inherits a **ComponentDocker**; and carries many Components.

In Osmium, they are treated as **Even Entities** and can be loaded in parallel like so :

    Osmium

        Loaded Scene A

        Loaded Scene B

Both Scene A and B will receive events independent of each other. It is also good to note that Components cannot exist at the **Scene-Level** (parallel of Scenes, like how A is to B). Components can only exist as a child of a Scene or a Child of a Component in a Scene.

## Usage

Using a Scene is easy! Simply call **Osmium.AddScene(string name)** to create a new Scene! Or you can add your own with **Osmium.AddScene(Scene scene)**. The reason you might want to add your own is to allow for custom scene types. If you have seen [Components](../Component/Component.md), then you might know that Osmium **supports** external inheritance! It's as easy as inheriting Scene, and using AddScene() to make your own Custom Scene Code!

For further guidance, refer to the **API** documentation! With **Osmium** you are able to Add, Get, and Remove Scenes.

---

---
# End Of Documentation
### Code and Documentation by Avery Norris

<p align="center">
  <img src="Osmium.png" width="150px" style="image-rendering: pixelated; align-content: center;" alt="OsmiumLogo">
</p>
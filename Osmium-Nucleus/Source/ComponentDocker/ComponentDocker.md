# Component Docker

### Code and Documentation by Avery Norris

---

One major and neccessary requirement of an **EC/ECS** [(Entity Component System)](https://en.wikipedia.org/wiki/Entity_component_system) is **Nested-Components**;
In **Osmium's** own **ECS** we have two objects that need to be able to hold **Children-Components**

    Scenes

    Components

Originally, both classes carried methods to store **Components** as children; However this was extremely redundant, and **Component-Docker** was created to handle
this in an organised fashion!

## Purpose

Called a **Docker** for short; the **Component-Docker** carries **three main functionalities**

    Storing Children Component

    Providing an Interface to Modify Children

    Transferring Events to the appropriate Components


 - **Storing Children** is the **Main Job** of a Docker. Both Scenes and Components can host children. And the docker inheritance is what
accommodates this. It uses a HashSet internally, and hashes Components in a list and two Dictionaries based on Priority, Type and Tags 


 - Osmium Provides an **Extensive Interface** to work with Components. You can check the API to see the MANY overloads for adding, getting, removing and moving Components provided.


 - **Transferring Events** is the final usage for the **Docker**. When an Event is sent in **Osmium**, it is passed down between Dockers via ChainEvent(). Dockers
  call all the Dockers below themselves, and their inherited type. This is stopped if a Docker is a Component that is not enabled, or we reach the leaves of all branches in the **Component-Tree**.

---

---
# End Of Documentation
### Code and Documentation by Avery Norris

<p align="center">
  <img src="Osmium.png" width="150px" style="image-rendering: pixelated; align-content: center;" alt="OsmiumLogo">
</p>
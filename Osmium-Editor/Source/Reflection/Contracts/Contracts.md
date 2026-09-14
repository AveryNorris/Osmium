A common issue in Osmium is that Editor Modules and Runtime modules have trouble referencing one another.

While Runtime modules should NEVER attempt to indirectly or directly reference an Editor module, 
it makes good sense to try and do the opposite.

Consider a case example

We have a Runtime module named "Renderer" which provides graphics to the Game, and
for the sake of simplicity, we will assume that the renderer always renders to fullscreen

Now we would like to embed that view into the editor, how do we do that? Well we need an Editor module,
that defines a window, and then calls the Runtime module to reference in the
bounds of that UI window.

Editor modules cannot directly reference runtime modules, so we use Contract methods

The first type is an RuntimeModuleEvent, this calls a void to all static methods in an assambly that use an attribute of the type of event
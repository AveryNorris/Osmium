using System.Drawing;
using OsmiumNucleus;
using OsmiumRadium;
using RadiumTest2;


Osmium.Initialize();

Text.DefaultFont = new Font("/Users/averynorris/Programming/Radium-Test2/RadiumFonts/ProggyClean.radfont");
Text.DefaultColor = Palette.TextHigh;

Button.DefaultBackgroundColor = Palette.Secondary;
Button.DefaultBackgroundHoverColor = Palette.Secondary;
Button.DefaultBackgroundHeldColor = Palette.SecondaryActive;


Osmium.AddScene("Hello").Add<RadiumTestRunner>();

Osmium.Run();
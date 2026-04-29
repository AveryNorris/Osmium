using System.Collections;
using System.Drawing;
using System.Numerics;
using OsmiumNucleus;
using OsmiumRadium;
using RadiumTest2;

namespace OsmiumEditor;

public class DebugConsole : RadiumElement
{
    //56.69

    public const float Width = 65f;
    public const float Height = 56.69f * r169;

    //0 = console
    //1 = files
    public int currentMenu = 0;
    
    float scroll = 0;


    protected override void Draw() {
        var BackgroundBox = new Box(new Transform(pos: new Vector2(0, 100 - Height), size: new Vector2(Width, Height)), color: Palette.BackgroundLow);

        var DividerLine = new Box(new Transform(pos: new Vector2(0, 100 - Height), size: new Vector2(Width, .125f)), color: Palette.BackgroundHigh);

        var DebugButton = new Button(new Transform(pos: new Vector2(0, 100 - Height), size: new Vector2(7, 3.125f)), new Text("Console"));
        if (currentMenu == 0)
        {
            //add elevated colors
            DebugButton.backgroundColor = Palette.SecondaryActive;
            DebugButton.backgroundHoverColor = Palette.SecondaryActive;
        }

        var Files = new Button(new Transform(pos: new Vector2(7.25f, 100 - Height), size: new Vector2(7, 3.125f)), new Text("Files"));
        if (currentMenu == 1)
        {
            Files.backgroundColor = Palette.SecondaryActive;
            Files.backgroundHoverColor = Palette.SecondaryActive;
        }


        var ButtonDivider = new Box(new Transform(pos: new Vector2(0, (100 - Height) + 3.125f), size: new Vector2(Width, .125f)), Palette.Secondary);

        if (Files.Active())
            currentMenu = 1;
        if (DebugButton.Active())
            currentMenu = 0;
        
        Vector2 min = new Vector2(0, 100 - Height);
        Radium.SetClippingBounds(new Vector2(0, 100 - Height + 1 + 4), min + new Vector2(Width, Height));

        scroll += Backend.ScrollDeltaY;

        float scrollMax = 0;
        //asumes msg is accurate, lock it down in debug
        foreach (KeyValuePair<DebugMessage, int> Msg in Debug.Stack)
        {
            scrollMax += 1 + Msg.Key.Values.Length * .5f;
        }        
        scroll = Math.Clamp(scroll, -4f * scrollMax, 0f);
        //todo: gross code

        if (currentMenu == 0)
        {
            float count = 0;
            foreach (KeyValuePair<DebugMessage, int> Entry in Debug.Stack)
            {
                Vector2 Pos = new Vector2(1.5f, 100 - Height + 5 + 4 * count + scroll);
                
                DebugMessage debugMessage = Entry.Key;

                string output = (Entry.Value > 1 ? Entry.Value + " : " : string.Empty) + debugMessage.Message;

                for (int i = 0; i < debugMessage.Values.Length; i++) {
                    output += "\n   " + debugMessage.Parameters[i] + " : " + debugMessage.Values[i];
                    count += .5f;
                }

                count++;
                var Text = new Text(output, pos: Pos);
            }
        }
        
        Radium.SetClippingBounds(Vector2.Zero, Vector2.One * 100);


        //var Header = new Box(new Transform(pos: new Vector2((100 - Size) - Offset, 0), size: new Vector2(Size, 6.75f)), color: Palette.Secondary);
        //var HeaderText = new Text("Hierarchy", pos: new Vector2(((100 - Size) + .5f) - Offset, 4.5f), spacing: new Vector2(.285f, 1), size: 1.6f)
    }
}
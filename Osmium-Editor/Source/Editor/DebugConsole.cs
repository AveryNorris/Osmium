using System.Collections;
using System.Drawing;

using System.Text;
using OsmiumNucleus;
using OsmiumRadium;
/*

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
        //todo: optimize holy
        var BackgroundBox = new Box(new Bounds(pos: new Vector2(0, 100 - Height), size: new Vector2(Width, Height)), color: Palette.BackgroundLow);

        var DividerLine = new Box(new Bounds(pos: new Vector2(0, 100 - Height), size: new Vector2(Width, .125f)), color: Palette.BackgroundHigh);

        var DebugButton = new Button(new Bounds(pos: new Vector2(0, 100 - Height), size: new Vector2(7, 3.125f)), new Text("Console"));
        if (currentMenu == 0)
        {
            //add elevated colors
            DebugButton.backgroundColor = Palette.SecondaryActive;
            DebugButton.backgroundHoverColor = Palette.SecondaryActive;
        }

        var Files = new Button(new Bounds(pos: new Vector2(7.25f, 100 - Height), size: new Vector2(7, 3.125f)), new Text("Files"));
        if (currentMenu == 1)
        {
            Files.backgroundColor = Palette.SecondaryActive;
            Files.backgroundHoverColor = Palette.SecondaryActive;
        }


        var ButtonDivider = new Box(new Bounds(pos: new Vector2(0, (100 - Height) + 3.125f), size: new Vector2(Width, .125f)), Palette.Secondary);

        if (Files.Active())
            currentMenu = 1;
        if (DebugButton.Active())
            currentMenu = 0;
        
        Vector2 min = new Vector2(0, 100 - Height);
        
        //todo: lag after here but also a good chunk before
        
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

        //todo: retained UI?

        StringBuilder output = new StringBuilder();
        
        if (currentMenu == 0)
        {
            float count = 0;
            foreach (KeyValuePair<DebugMessage, int> Entry in Debug.Stack)
            {
                DebugMessage debugMessage = Entry.Key;

                output.Append((Entry.Value > 1 ? Entry.Value + " : " : string.Empty) + debugMessage.Message);

                for (int i = 0; i < debugMessage.Values.Length; i++) {
                    output.Append("\n   " + debugMessage.Parameters[i] + " : " + debugMessage.Values[i]);
                    count += .5f;
                }

                output.Append("\n\n");

                count++;
            }
        }
        //todo: handle compilation error, add safemode and encapsulate the editor more
        
        var Text = new Text(output.ToString(), pos: new Vector2(1.5f, 100 - Height + 5 + scroll));
        
        Radium.SetClippingBounds(Vector2.Zero, Vector2.One * 100);


        //var Header = new Box(new Bounds(pos: new Vector2((100 - Size) - Offset, 0), _size: new Vector2(Size, 6.75f)), _color: Palette.Secondary);
        //var HeaderText = new Text("Hierarchy", pos: new Vector2(((100 - Size) + .5f) - Offset, 4.5f), _spacing: new Vector2(.285f, 1), _size: 1.6f)
    }
}
*/
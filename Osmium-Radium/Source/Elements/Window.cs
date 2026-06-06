using OpenTK.Windowing.Common.Input;
using OpenTK.Windowing.GraphicsLibraryFramework;
using OsmiumNucleus;

namespace OsmiumRadium;

public abstract class Window : RetainedElement
{
    
    public Rect Rect;
    
    private bool resizingMax;
    private bool resizingMin;
    private bool resizingTopRight;
    private bool resizingBottomLeft;

    private bool resizingTopEdge;
    private bool resizingBottomEdge;
    private bool resizingLeft;
    private bool resizingRight;

    private bool movingWindow;

    private bool setCursor = false;
    
    //todo: class that oversees cursor mode


    protected internal override void Draw() {
        NestedRegion region = Region().Rect(Rect);

        
        Vector2 adjustedScalingFactor = new Vector2(1);
        adjustedScalingFactor.x /= Backend.WindowWidthHeightRatio;


        Rect maxRect = Rect.FromCorners(Rect.max, Rect.max - adjustedScalingFactor);

        Rect minRect = Rect.FromCorners(Rect.min, Rect.min + adjustedScalingFactor);


        //todo: clean up this attrocious mess and make sure bounds are fixed, make a constructor that works with two points A and B instead of min and max and calculate the min and maxes in the constructor
        //todo: also this is broken i think haha
        Vector2 topRightVector2 = new Vector2(Rect.max.x, Rect.min.y);
        Vector2 topRightMin = topRightVector2 - new Vector2(adjustedScalingFactor.x, 0);
        Vector2 topRightMax = topRightVector2 + new Vector2(0, adjustedScalingFactor.y);

        Rect topRight = Rect.FromCorners(topRightMin, topRightMax);


        Vector2 bottomLeftVector = new Vector2(Rect.min.x, Rect.max.y);
        Vector2 bottomMinVector = bottomLeftVector - new Vector2(0, adjustedScalingFactor.y);
        Vector2 bottomMaxVector = bottomLeftVector + new Vector2(adjustedScalingFactor.x, 0);

        Rect bottomLeft = Rect.FromCorners( bottomMinVector, bottomMaxVector);

        Rect topBar = Rect.FromCorners( new Vector2(minRect.max.x, minRect.min.y),
            new Vector2(topRight.min.x, topRight.max.y));
        Rect bottomBar = Rect.FromCorners(new Vector2(bottomLeft.max.x, bottomLeft.min.y),
            new Vector2(maxRect.min.x, maxRect.max.y));

        Rect leftBar = Rect.FromCorners( new Vector2(minRect.min.x, minRect.max.y),
            new Vector2(bottomLeft.max.x, bottomLeft.min.y));
        Rect rightBar = Rect.FromCorners(new Vector2(topRight.min.x, topRight.max.y),
            new Vector2(maxRect.max.x, maxRect.min.y));

        //todo: make the operators go in both directions
        Rect innerDragBar = Rect.FromCenterSize(Rect.center, Rect.size - (adjustedScalingFactor * 2f));
        innerDragBar.center = Rect.center;

        Rect innerDrawBarMask = innerDragBar;
        innerDrawBarMask.size -= adjustedScalingFactor * 2f;
        innerDrawBarMask.center = Rect.center;

        if (innerDragBar.MouseDown(MouseButton.Left) && !innerDrawBarMask.MouseDown(MouseButton.Left))
        {
            movingWindow = true;
        }




        if (DebugFlags.DebugWindows)
        {
            Region(() =>
            {
                Box().Size(100).Color(Palette.White);
                Box().Rect(maxRect).Color(resizingMax ? Palette.Red : Palette.Blue);
                Box().Rect(minRect).Color(resizingMin ? Palette.Red : Palette.Blue);
                Box().Rect(topRight).Color(resizingTopRight ? Palette.Red : Palette.Blue);
                Box().Rect(bottomLeft).Color(resizingBottomLeft ? Palette.Red : Palette.Blue);

                Box().Rect(topBar).Color(resizingTopEdge ? Palette.Red : Palette.Green);
                Box().Rect(bottomBar).Color(resizingBottomEdge ? Palette.Red : Palette.Green);
                Box().Rect(leftBar).Color(resizingLeft ? Palette.Red : Palette.Green);
                Box().Rect(rightBar).Color(resizingRight ? Palette.Red : Palette.Green);

                Box().Rect(innerDragBar).Color(movingWindow ? Color.FromRgb(255, 255, 0) : Color.FromRgb(255, 0, 255));
                Box().Rect(innerDrawBarMask).Color(movingWindow ? Palette.Transparent : Color.FromRgb(0, 255, 255));
            }).Depth(-.9999f);
        }
        
        //todo: add custom error messages for duplicate modules that cause it to be reloaded


        //todo: undo redo in the editor


        if (minRect.MouseDown(MouseButton.Left)) resizingMin = true;
        if (maxRect.MouseDown(MouseButton.Left)) resizingMax = true;
        if (topRight.MouseDown(MouseButton.Left)) resizingTopRight = true;
        if (bottomLeft.MouseDown(MouseButton.Left)) resizingBottomLeft = true;

        if (topBar.MouseDown(MouseButton.Left)) resizingTopEdge = true;
        if (bottomBar.MouseDown(MouseButton.Left)) resizingBottomEdge = true;
        if (leftBar.MouseDown(MouseButton.Left)) resizingLeft = true;
        if (rightBar.MouseDown(MouseButton.Left)) resizingRight = true;
        
        if(setCursor) { 
            Osmium.Window.Cursor = MouseCursor.Default;
            setCursor = false;
        }

        if (bottomLeft.MouseInBounds() || topRight.MouseInBounds()) {
            setCursor = true;
            Osmium.Window.Cursor = MouseCursor.ResizeNESW;
         } else if (minRect.MouseInBounds() || maxRect.MouseInBounds()) {
            setCursor = true;
            Osmium.Window.Cursor = MouseCursor.ResizeNWSE;
        }
        else if (topBar.MouseInBounds() || bottomBar.MouseInBounds())
        {
            setCursor = true;
            Osmium.Window.Cursor = MouseCursor.ResizeNS;
        } else if (leftBar.MouseInBounds() || rightBar.MouseInBounds()) {
            setCursor = true;
            Osmium.Window.Cursor = MouseCursor.ResizeEW;
        } else if (innerDragBar.MouseInBounds() && !innerDrawBarMask.MouseInBounds())
        {
            setCursor = true;
            Osmium.Window.Cursor = MouseCursor.Crosshair;
        }
        

        if (resizingMax)
        {
            Rect.max = Input.MousePos;
        }

        if (resizingMin)
        {
            Vector2 min = Input.MousePos;

            
            Rect = Rect.FromCorners(Rect.max, min);
        }

        if (resizingTopRight)
        {
            Vector2 max = new Vector2(Input.MousePos.x, Rect.max.y);
            Vector2 min = new Vector2(Rect.min.x, Input.MousePos.y);

            Rect = Rect.FromCorners(max, min);
        }

        if (resizingBottomLeft)
        {
            
            Vector2 max = new Vector2(Rect.max.x, Input.MousePos.y);
            Vector2 min = new Vector2(Input.MousePos.x, Rect.min.y);
            
            Rect = Rect.FromCorners(max, min);
        }

        if (resizingTopEdge) {
            Rect.min = new Vector2(Rect.min.x, Input.MousePos.y);
        }
        
        if (resizingBottomEdge) {
            Rect.max = new Vector2(Rect.max.x, Input.MousePos.y);
        }
        
        if (resizingLeft) {
            Rect.min = new Vector2(Input.MousePos.x, Rect.min.y);
        }
        
        if (resizingRight) {
            Rect.max = new Vector2(Input.MousePos.x, Rect.max.y);
        }
        
        if (Input.MouseUp(MouseButton.Left))
        {
            resizingTopRight = false;
            resizingBottomLeft = false;
            resizingMin = false;
            resizingMax = false;
            resizingTopEdge = false;
            resizingBottomEdge = false;
            resizingLeft = false;
            resizingRight = false;
            movingWindow = false;
        }
        

        if (movingWindow) {
            Rect.pos += Input.MouseDelta;
        }
        
        if (Rect.size.x < 5)
        {
            movingWindow = false;
            Rect.size = new Vector2(5, Rect.size.y);
            //todo: weird movement when the window hits the size limit
        }
        
        //todo: cull clicking to the newest member
        
        if (Rect.size.y < 5)
        {
            movingWindow = false;
            Rect.size = new Vector2(Rect.size.x, 5);
        }
        
        if (resizingBottomEdge || resizingBottomLeft || resizingLeft || resizingMax || resizingMin || resizingRight ||
            resizingTopEdge || resizingTopRight || movingWindow)
        {
            region.Depth(IDepthElementExtensions.DraggingDepth);
        }
        
        DrawWindow();
        
        Exit();
    }

    protected abstract void DrawWindow();
    
    //todo: create a temporary list of elements that are loaded outside the assembly
}
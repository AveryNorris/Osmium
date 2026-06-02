using OpenTK.Windowing.Common.Input;
using OpenTK.Windowing.GraphicsLibraryFramework;
using OsmiumNucleus;

namespace OsmiumRadium;

public abstract class Window : RetainedElement
{
    
    public Bounds bounds;
    
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
        Region("window").Bounds(bounds);


        Vector2 adjustedScalingFactor = new Vector2(1);
        adjustedScalingFactor.x /= Backend.WindowWidthHeightRatio;


        Bounds maxBounds = new Bounds(max: bounds.max, min: bounds.max - adjustedScalingFactor);

        Bounds minBounds = new Bounds(min: bounds.min, max: bounds.min + adjustedScalingFactor);


        //todo: clean up this attrocious mess and make sure bounds are fixed, make a constructor that works with two points A and B instead of min and max and calculate the min and maxes in the constructor
        //todo: also this is broken i think haha
        Vector2 topRightVector2 = new Vector2(bounds.max.x, bounds.min.y);
        Vector2 topRightMin = topRightVector2 - new Vector2(adjustedScalingFactor.x, 0);
        Vector2 topRightMax = topRightVector2 + new Vector2(0, adjustedScalingFactor.y);

        Bounds topRight = new Bounds(min: topRightMin, max: topRightMax);


        Vector2 bottomLeftVector = new Vector2(bounds.min.x, bounds.max.y);
        Vector2 bottomMinVector = bottomLeftVector - new Vector2(0, adjustedScalingFactor.y);
        Vector2 bottomMaxVector = bottomLeftVector + new Vector2(adjustedScalingFactor.x, 0);

        Bounds bottomLeft = new Bounds(min: bottomMinVector, max: bottomMaxVector);

        Bounds topBar = new Bounds(min: new Vector2(minBounds.max.x, minBounds.min.y),
            max: new Vector2(topRight.min.x, topRight.max.y));
        Bounds bottomBar = new Bounds(min: new Vector2(bottomLeft.max.x, bottomLeft.min.y),
            max: new Vector2(maxBounds.min.x, maxBounds.max.y));

        Bounds leftBar = new Bounds(min: new Vector2(minBounds.min.x, minBounds.max.y),
            max: new Vector2(bottomLeft.max.x, bottomLeft.min.y));
        Bounds rightBar = new Bounds(min: new Vector2(topRight.min.x, topRight.max.y),
            max: new Vector2(maxBounds.max.x, maxBounds.min.y));

        //todo: make the operators go in both directions
        Bounds innerDragBar = new Bounds(center: bounds.center, size: bounds.size - (adjustedScalingFactor * 2f));
        innerDragBar.center = bounds.center;

        Bounds innerDrawBarMask = innerDragBar;
        innerDrawBarMask.size -= adjustedScalingFactor * 2f;
        innerDrawBarMask.center = bounds.center;

        if (innerDragBar.MouseDown(MouseButton.Left) && !innerDrawBarMask.MouseDown(MouseButton.Left))
        {
            movingWindow = true;
        }




        if (DebugFlags.DebugWindows)
        {
            Box().Size(100).Color(Palette.White).Depth(.99f);
            Box().Bounds(maxBounds).Color(resizingMax ? Palette.Red : Palette.Blue).Depth(.99f);
            Box().Bounds(minBounds).Color(resizingMin ? Palette.Red : Palette.Blue).Depth(.99f);
            Box().Bounds(topRight).Color(resizingTopRight ? Palette.Red : Palette.Blue).Depth(.99f);
            Box().Bounds(bottomLeft).Color(resizingBottomLeft ? Palette.Red : Palette.Blue).Depth(.99f);

            Box().Bounds(topBar).Color(resizingTopEdge ? Palette.Red : Palette.Green).Depth(.99f);
            Box().Bounds(bottomBar).Color(resizingBottomEdge ? Palette.Red : Palette.Green).Depth(.99f);
            Box().Bounds(leftBar).Color(resizingLeft ? Palette.Red : Palette.Green).Depth(.99f);
            Box().Bounds(rightBar).Color(resizingRight ? Palette.Red : Palette.Green).Depth(.99f);

            Box().Bounds(innerDragBar).Color(movingWindow ? Color.FromRgb(255, 255, 0) : Color.FromRgb(255, 0, 255)).Depth(.99f);
            Box().Bounds(innerDrawBarMask).Color(movingWindow ? Palette.Transparent : Color.FromRgb(0, 255, 255)).Depth(.99f);
        }
        
        //todo: add custom error messages for duplicate modules that cause it to be reloaded




        if (minBounds.MouseDown(MouseButton.Left)) resizingMin = true;
        if (maxBounds.MouseDown(MouseButton.Left)) resizingMax = true;
        if (topRight.MouseDown(MouseButton.Left)) resizingTopRight = true;
        if (bottomLeft.MouseDown(MouseButton.Left)) resizingBottomLeft = true;

        if (topBar.MouseDown(MouseButton.Left)) resizingTopEdge = true;
        if (bottomBar.MouseDown(MouseButton.Left)) resizingBottomEdge = true;
        if (leftBar.MouseDown(MouseButton.Left)) resizingLeft = true;
        if (rightBar.MouseDown(MouseButton.Left)) resizingRight = true;
        
        if(setCursor) { 
            Osmium.Context.Cursor = MouseCursor.Default;
            setCursor = false;
        }

        if (bottomLeft.MouseInBounds() || topRight.MouseInBounds()) {
            setCursor = true;
            Osmium.Context.Cursor = MouseCursor.ResizeNESW;
         } else if (minBounds.MouseInBounds() || maxBounds.MouseInBounds()) {
            setCursor = true;
            Osmium.Context.Cursor = MouseCursor.ResizeNWSE;
        }
        else if (topBar.MouseInBounds() || bottomBar.MouseInBounds())
        {
            setCursor = true;
            Osmium.Context.Cursor = MouseCursor.ResizeNS;
        } else if (leftBar.MouseInBounds() || rightBar.MouseInBounds()) {
            setCursor = true;
            Osmium.Context.Cursor = MouseCursor.ResizeEW;
        } else if (innerDragBar.MouseInBounds() && !innerDrawBarMask.MouseInBounds())
        {
            setCursor = true;
            Osmium.Context.Cursor = MouseCursor.Crosshair;
        }
        

        if (resizingMax)
        {
            bounds.max = Input.MousePos;
        }

        if (resizingMin)
        {
            Vector2 min = Input.MousePos;

            
            bounds = new Bounds(max: bounds.max, min: min);
        }

        if (resizingTopRight)
        {
            Vector2 max = new Vector2(Input.MousePos.x, bounds.max.y);
            Vector2 min = new Vector2(bounds.min.x, Input.MousePos.y);

            bounds = new Bounds(max: max, min: min);
        }

        if (resizingBottomLeft)
        {
            
            Vector2 max = new Vector2(bounds.max.x, Input.MousePos.y);
            Vector2 min = new Vector2(Input.MousePos.x, bounds.min.y);
            
            bounds = new Bounds(max: max, min: min);
        }

        if (resizingTopEdge) {
            bounds.min = new Vector2(bounds.min.x, Input.MousePos.y);
        }
        
        if (resizingBottomEdge) {
            bounds.max = new Vector2(bounds.max.x, Input.MousePos.y);
        }
        
        if (resizingLeft) {
            bounds.min = new Vector2(Input.MousePos.x, bounds.min.y);
        }
        
        if (resizingRight) {
            bounds.max = new Vector2(Input.MousePos.x, bounds.max.y);
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
            bounds.pos += Input.MouseDelta;
        }
        
        if (bounds.size.x < 5)
        {
            movingWindow = false;
            bounds.size = new Vector2(5, bounds.size.y);
            //todo: weird movement when the window hits the size limit
        }
        
        //todo: cull clicking to the newest member
        
        if (bounds.size.y < 5)
        {
            movingWindow = false;
            bounds.size = new Vector2(bounds.size.x, 5);
        }
        
        DrawWindow();
        
        Exit();
    }

    protected abstract void DrawWindow();
    
    //todo: create a temporary list of elements that are loaded outside the assembly
}
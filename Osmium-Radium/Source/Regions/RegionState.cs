using OsmiumNucleus;

namespace OsmiumRadium;

/// <summary> Represents the current state of regions as they are being drawn, and gives methods to modify the target region </summary>
public static class RegionState
{
    
    
    
    /// <summary> All the regions that are currently in scope, when you enter a region
    /// it is added to the scope and as you exit it is forgotten. The endmost region always
    /// represents the current region that is active. </summary>
    public static List<NestedRegion> RegionScope = [];


    
    /// <summary> Resets the focus on all the current regions, this is called by the backend
    /// after every single retained element draw to prevent regions leaking between elements
    /// and encapsulate them safely </summary>
    public static void ResetFocus() {
        if (RegionScope.Count > 0) {
            Debug.Error("Region focus is lopsided! Did you forget to exit a region?");
        }
        
        RegionScope.Clear();
    }



    /// <summary> Directs focus towards a given region and appends it to the region scope.
    /// The region inputted becomes the new active region, if the new region does not belong to
    /// the current region it will throw! </summary>
    /// <param name="__region"></param>
    public static void Focus(NestedRegion __region) {
        if (!Current.Contains(__region)) {
            Debug.Error("Attempting to focus on a new region that does not belong to the current active region! You must exit this region or nest the new one within it to preserve hierarchy!");
            return;
        }
        
        RegionScope.Add(__region);
    }

    /// <summary> Exits the current region and steps up the region scope ladder! If you are already at the top of the
    /// region scope it will throw </summary>
    public static void Exit() {
        if (RegionScope.Count == 0) Debug.Error("No region to exit! You are already in the base region.");
        else RegionScope.RemoveAt(RegionScope.Count - 1);
    }

    
    /// <summary> The current region that Radium is drawing to </summary>
    public static Region Current => RegionScope.Count > 0 ? RegionScope[^1] : Backend.BaseRegion;



    /// <summary> The current region that Radium is drawing to </summary>
    public static Bounds CurrentRegionBounds => (Current is NestedRegion region) ? region._bounds : new Bounds(size: new Vector2(100, 100));
}

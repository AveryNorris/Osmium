namespace OsmiumRadium;

//Palettes are encoded like so with each index at the associated byte
//======================================
//| 0 0 0 0 0 0 0 0 | 0  0  0  0  0  0  0  0 |
//|     HEADER      |          TYPE          |
//| highest        ==>                 lowest|
//============================================

//the first byte on the left is a header, it describes the type of color, so primary secondary etc
//the second represents the type of color, so hover, etc

//keep in mind both local byte is shifted if it isn't the first one!
//so the header is shifted by 8 for example



//- constant
//primary = 1
//secondary = 2
//tertiary = 3
//background = 4
//border = 5
//text = 6
//error = 7
//misc = 8

//constants

//- unknown (magenta)
//9 white
//10 black
//11 red
//12 green
//13 blue
//14 transparent

//- default
//selected = 0
//active = 1
//disabled = 2
//deselected = 3
//low = 4
//medium = 5
//high = 6
//foreground = 7

//the TYPE key has an alternate if the header is constant

//this goes without saying, but you can use the enumerator by 
[Flags]
public enum Palette : ushort
{
    
    
    //HEADER BYTE (SHIFTED BY 8)

    
    Primary = 1<<8,
    Secondary = 2<<8,
    Tertiary = 3<<8,
    Background = 4<<8,
    Border = 5<<8,
    Text = 6<<8,
    Error = 7<<8,
    Misc = 8<<8,
    White = 9<<8,
    Black = 10<<8,
    Red = 11<<8,
    Green = 12<<8,
    Blue = 13<<8,
    Transparent = 14<<8,
    
    
    //TYPE BYTE
    
    
    Selected = 1,
    Active = 2,
    Disabled = 3,
    Deselected = 4,
    Low = 5,
    Medium = 6,
    High = 7,
    Foreground = 8,
}
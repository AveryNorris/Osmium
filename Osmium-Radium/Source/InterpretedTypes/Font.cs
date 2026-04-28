using System.Collections.Immutable;
using System.Numerics;
using OpenTK.Graphics.OpenGL;
using OsmiumNucleus;
using StbImageSharp;

namespace OsmiumRadium;

public class Font
{


    public Texture texture;
    
    /// <summary> represents the list of character ranges inclusive, for instance a default ascii font may have the list as [32, 147] </summary>
    private IReadOnlyList<int> CharRanges = [];
    public readonly int GlyphSize = -1;
    public readonly int CharsPerRow = -1;

    public Dictionary<string, (IReadOnlyList<int> CharRanges, int GlyphSize, int CharsPerRow)> FontMemory = [];
    
    public Font(Stream __fontFile, int __glyphSize, int __charsPerRow, List<int> __charRanges)
    {
        
        GlyphSize = __glyphSize;
        CharsPerRow = __charsPerRow;
        
        CharRanges = __charRanges;
        
        texture = new Texture(__fontFile);
    }

    //returns the UV rect for a given char stored in the font
    public Vector4 this[char __c] {
        get {
            int charIndex = 0;

            for (int i = 0; i < CharRanges.Count; i += 2) {
                if (i == CharRanges.Count - 1) {
                    Debug.LogError("A given char is not within the font's valid range! ", ["Char"], [__c.ToString()]);
                    return Vector4.Zero;
                }

                if (__c >= CharRanges[i] && __c <= CharRanges[i + 1]) {
                    charIndex = __c - CharRanges[i];

                    for (int n = 0; n < i - 1; n += 2) {
                        //+ 1 should be removed maybe idk
                        charIndex += CharRanges[n + 1] - CharRanges[n] + 1;
                    }

                    break;
                }
            }

            float uvX = charIndex % CharsPerRow;
            float uvY = MathF.Floor((float)charIndex / CharsPerRow);

            float UVScaleX = (float)GlyphSize / texture.Width;
            float UVScaleY = (float)GlyphSize / texture.Height;

            return new Vector4(
                uvX * UVScaleX,
                uvY * UVScaleY,
                uvX * UVScaleX + UVScaleX, 
                uvY * UVScaleY + UVScaleY
            );
        }
    }
    //todo: idisposable
}
using System.Collections.Immutable;
using System.Numerics;
using OpenTK.Graphics.OpenGL;
using OsmiumNucleus;
using StbImageSharp;

namespace OsmiumRadium;

public class Font
{


    public Texture texture;
    
    /// <summary> represents the list of character ranges inclusive, for instance a default ascii _font may have the list as [32, 147] </summary>
    private IReadOnlyList<int> CharRanges = [];
    public readonly int GlyphSize = -1;
    public readonly int CharsPerRow = -1;

    public readonly int[] charIndexes = [];

    public Dictionary<string, (IReadOnlyList<int> CharRanges, int GlyphSize, int CharsPerRow)> FontMemory = [];
    
    public Font(Stream __fontFile, int __glyphSize, int __charsPerRow, List<int> __charRanges)
    {
        
        GlyphSize = __glyphSize;
        CharsPerRow = __charsPerRow;
        
        CharRanges = __charRanges;
        
        texture = new Texture(__fontFile);

        //todo: check that char ranges 0 and ^1 are min and max respectively
        int count = 0;
        List<int> indexes = [];

        for (int i = 0; i < CharRanges[0]; i++) {
            indexes.Add(-1);
        }
        
        for (int i = CharRanges[0]; i <= CharRanges[^1]; i++) {

            bool found = false;
            for (int n = 0; n < CharRanges.Count - 1; n++) {
                if (i >= CharRanges[n] && i <= CharRanges[n + 1]) {
                    found = true;
                }
            }

            if (found) {
                indexes.Add(count);
                count++;
            } else {
                indexes.Add(-1);
            }
        }

        charIndexes = indexes.ToArray();
        
        
        Debug.Log(string.Join(" , ", indexes.ToImmutableArray()));

        //TODO: PRECOMPUTE LOOKUP TABLES!!!
    }

    //returns the UV rect for a given char stored in the _font
    public Vector4 this[char __c] {
        get {
            int charIndex = charIndexes[__c];

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
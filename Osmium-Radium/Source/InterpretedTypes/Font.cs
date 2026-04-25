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
    
    public Font(string __filePath)
    {

        if (FontMemory.TryGetValue(__filePath, out (IReadOnlyList<int> CharRanges, int GlyphSize, int CharsPerRow) FontInformation)) {
            CharRanges = FontInformation.CharRanges;
            GlyphSize = FontInformation.GlyphSize;
            CharsPerRow = FontInformation.CharsPerRow;
            return;
        }

        if (Path.GetFileNameWithoutExtension(__filePath) == "radfont") {
            Debug.LogError("Font files must be of type .radfont!");
            return;
        }
        
        string path = string.Empty;

        int glyphSizeCount = 0;
        int charsPerRowCount = 0;
        
        List<int> Ranges = [];
        
        #region File Parsing
        
        string[] FontStatements = File.ReadAllText(__filePath).Split('\n');

        foreach (string statement in FontStatements) {

            string[] values = statement.Split('\"');

            if (values.Length <= 2) {
                continue;
            }

            string identifier = values[0].Trim().ToUpper();

            if (identifier == "GLYPHSIZE") {
                glyphSizeCount++;
                if (!int.TryParse(values[1], out GlyphSize)) {
                    Debug.LogError("Invalid glyph size!", ["FilePath"], [__filePath]);
                    return;
                }
            } else if (identifier == "CHARSPERROW") {
                charsPerRowCount++;
                if (!int.TryParse(values[1], out CharsPerRow)) {
                    Debug.LogError("Invalid chars per row!", ["FilePath"], [__filePath]);
                    return;
                }
            } else if (identifier == "PATH") {
                path = values[1];
            } else if (identifier == "GLYPHRANGES") {
                string[] listValues = values[1].Split(',');

                foreach (string listValue in listValues) {
                    if (int.TryParse(listValue, out int rangeValue)) Ranges.Add(rangeValue);
                    else {
                        Debug.LogError("Invalid font range", ["FilePath"], [__filePath]);
                        return;
                    }
                }
            }
        }

        if (glyphSizeCount == 0) {
                Debug.LogError("Radium Font file does not contain a glyph size parameter!", ["FilePath"], [__filePath]);
                return;
        }else if (glyphSizeCount > 1) {
            Debug.LogError("Radium Font file contains too many glyph size parameters!", ["FilePath"], [__filePath]);
            return;
        }
            
        if (charsPerRowCount == 0) {
            Debug.LogError("Radium Font file does not contain a chars per row parameter!", ["FilePath"], [__filePath]);
            return;
        }else if (charsPerRowCount > 1) {
            Debug.LogError("Radium Font file contains too many chars per row parameters!", ["FilePath"], [__filePath]);
            return;
        }

        if (Ranges.Count < 2) {
            Debug.LogError("Radium Font file does not have enough numbers for the char range!", ["FilePath"], [__filePath]);
            return;
        }

        if (GlyphSize <= 0)
        {
            Debug.LogError("Radium font file has an invalid glyph size! It cannot be under or equal to zero", ["FilePath"], [__filePath]);
            return;
        }
            
        if (CharsPerRow <= 0)
        {
            Debug.LogError("Radium font file has an invalid chars per row! It cannot be under or equal to zero", ["FilePath"], [__filePath]);
            return;
        }

        if (!File.Exists(path))
        {
            Debug.LogError("The given path in the Radium font file does not exist!", ["FilePath"], [__filePath]);
            return;
        }

        CharRanges = Ranges;
        
        #endregion
        
        texture = new Texture(path);
        
        FontMemory.Add(__filePath, (CharRanges, GlyphSize, CharsPerRow));
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
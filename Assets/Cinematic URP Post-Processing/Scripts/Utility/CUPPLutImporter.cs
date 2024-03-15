#if UNITY_EDITOR
using UnityEditor.AssetImporters;
using UnityEngine;
using UnityEngine.Rendering;
using System;
using System.Collections;
using System.Text;
using System.IO;
using System.Collections.Generic;
using System.Globalization;

namespace PRISM.Utils { 
[ScriptedImporter(0, ".cube", AllowCaching = true)]
public class CUPPLutImporter : ScriptedImporter
{
    public Texture2D t2d;

    static string FilterLine(string line)
    {
        var filtered = new StringBuilder();
        line = line.TrimStart().TrimEnd();
        int len = line.Length;
        int o = 0;

        while (o < len)
        {
            char c = line[o];
            if (c == '#') break; // Comment filtering
            filtered.Append(c);
            o++;
        }

        return filtered.ToString();
    }
    public static bool ParseCubeData(string ctxPath, out int lutSize, out Color[] pixels)
    {
        Texture3D texture3D = new Texture3D(1, 1, 1, TextureFormat.RGBA32, false);
        // Quick & dirty error utility
        bool Error(string msg)
        {
            //ctx.LogImportError(msg);
            Debug.LogWarning(msg);
            return false;
        }

        var lines = File.ReadAllLines(ctxPath);
        pixels = null;
        lutSize = -1;

        // Start parsing
        int sizeCube = -1;
        var table = new List<Color>();

        for (int i = 0; true; i++)
        {
            // EOF
            if (i >= lines.Length)
            {
                if (table.Count != sizeCube)
                    return Error("Premature end of file");

                break;
            }

            // Cleanup & comment removal
            var line = FilterLine(lines[i]);

            if (string.IsNullOrEmpty(line))
                continue;

            // Header data
            if (line.StartsWith("TITLE"))
                continue; // Skip the title tag, we don't need it

            if (line.StartsWith("LUT_3D_SIZE"))
            {
                var sizeStr = line.Substring(11).TrimStart();

                if (!int.TryParse(sizeStr, out var size))
                    return Error($"Invalid data on line {i}");

                //if (size < GlobalPostProcessSettings.k_MinLutSize || size > GlobalPostProcessSettings.k_MaxLutSize)
                //    return Error("LUT size out of range");

                lutSize = size;
                sizeCube = size * size * size;

                continue;
            }

            if (line.StartsWith("DOMAIN_"))
                continue; // Skip domain boundaries, haven't run into a single cube file that used them

            // Table
            var row = line.Split();

            if (row.Length != 3)
                return Error($"Invalid data on line {i}");

            var color = Color.black;

            for (int j = 0; j < 3; j++)
            {
                if (!float.TryParse(row[j], NumberStyles.Float, CultureInfo.InvariantCulture.NumberFormat, out var d))
                    return Error($"Invalid data on line {i}");

                color[j] = d;
            }

            table.Add(color);
        }

        if (sizeCube != table.Count)
            return Error($"Wrong table size - Expected {sizeCube} elements, got {table.Count}");

        pixels = table.ToArray();
        return true;
    }

    /*
    public static Texture2D FlipTexture(Texture2D original)
    {
        Texture2D flipped = new Texture2D(original.width, original.height);

        int xN = original.width;
        int yN = original.height;


        for (int i = 0; i < xN; i++)
        {
            for (int j = 0; j < yN; j++)
            {
                flipped.SetPixel(i, yN - j - 1, original.GetPixel(i, j));
            }
        }
        flipped.Apply(true);

        var bytes = flipped.EncodeToPNG();

        string path = AssetDatabase.GetAssetPath(original);
        System.IO.File.WriteAllBytes(path, bytes);

        AssetDatabase.Refresh();
        TextureImporter textureImporter = (TextureImporter)AssetImporter.GetAtPath(path);
        bool doImport = textureImporter.isReadable == false;
        if (textureImporter.mipmapEnabled == true)
        {
            doImport = true;
        }
        //if (textureImporter.textureFormat != TextureImporterFormat.AutomaticTruecolor) {
        //	doImport = true;
        //}

        if (doImport)
        {
            textureImporter.isReadable = true;
            textureImporter.mipmapEnabled = false;
            //textureImporter.textureFormat = TextureImporterFormat.AutomaticTruecolor;
            AssetDatabase.ImportAsset(path, ImportAssetOptions.ForceUpdate);
        }

        return AssetDatabase.LoadAssetAtPath(path, typeof(Texture2D)) as Texture2D;

        //return flipped;
    }*/

    public Texture2D FlipTexture(Texture2D original)
    {
        Texture2D flipped = new Texture2D(original.width, original.height);

        int xN = original.width;
        int yN = original.height;


        for (int i = 0; i < xN; i++)
        {
            for (int j = 0; j < yN; j++)
            {
                flipped.SetPixel(i, yN - j - 1, original.GetPixel(i, j));
            }
        }
        flipped.Apply(true);

        return flipped;
    }

    public override void OnImportAsset(AssetImportContext ctx)
    {
        if (ParseCubeData(ctx.assetPath, out var size, out var colors))
        {
            var width = size * size;
            var colorCount = colors.Length;

            t2d = new Texture2D(width, size, TextureFormat.RGBAFloat, false);

            var tempColors = new Color[colorCount];

            for (var i = 0; i < colorCount; i++)
            {
                // 1D to 3D Coords
                var X = i % size;
                var Y = (i / size) % size;
                var Z = i / (size * size);

                // 2D Coords
                // Shifting X by the width of a full tile, everytime we move to a new Z slice
                var targetX = X + Z * size;

                // 2D to 1D Coords
                var j = targetX + Y * width;

                // Put the color in a new place in the 2D texture
                tempColors[j] = colors[i];
            }

            t2d.SetPixels(tempColors);
            t2d.Apply();

            t2d = FlipTexture(t2d);

            ctx.AddObjectToAsset("CUPP_LUT_", t2d);
            ctx.SetMainObject(t2d);
        }
    }
}
}
#endif

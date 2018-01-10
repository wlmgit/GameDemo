using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;
using System.IO;

public class ArtistFontMaker {

    [MenuItem("Assets/ArtistFontMaker")]
    static void CreateArtistFont()
    {
        ArtistFontMaking();
    }

    private static void ArtistFontMaking()
    {
        string filePath = "";
        string fullFileName = EditorUtil.GetSelectFileName( ref filePath );
        string fileDir = Path.GetDirectoryName(filePath) + "/";
        //如果选择的不是艺术字配置，退出
        if ( !fullFileName.EndsWith("fnt") )
        {
            return;
        }
        string fileName = fullFileName.Split('.')[0];

        TextAsset artistTextAsset = AssetDatabase.LoadAssetAtPath<TextAsset>(filePath);
        if ( artistTextAsset.bytes == null )
            return ;
        Font customFont = new Font();
        FontTxt fontTxt = new FontTxt();
        string txtName = "";
        List<CharInfo> charInfos = GetCharInfo( artistTextAsset.bytes ,ref fontTxt,ref txtName);
        CharacterInfo[] characterInfos = new CharacterInfo[charInfos.Count];
        for (int i = 0; i < charInfos.Count ;i++)
        {
            CharInfo charInfo = charInfos[i];
            CharacterInfo characterInfo = new CharacterInfo();
            characterInfo.index = charInfo.index;

            float uvX = (float) charInfo.x / (float) fontTxt.texWidth;
            float uvY = 1 - (float)charInfo.y / (float)fontTxt.texHeight;
            float uvWidth = (float)charInfo.width/ (float)fontTxt.texWidth;
            float uvHeight = (-1) * (float)charInfo.height / (float)fontTxt.texHeight;

            characterInfo.uvBottomLeft = new Vector2(uvX,uvY);
            characterInfo.uvBottomRight = new Vector2(uvX+uvWidth, uvY);
            characterInfo.uvTopLeft = new Vector2(uvX, uvY+uvHeight);
            characterInfo.uvTopRight = new Vector2(uvX + uvWidth, uvY+uvHeight);
            characterInfo.minX = charInfo.offsetX;
            characterInfo.minY = (int)((float)charInfo.offsetY + (float)charInfo.height);
            characterInfo.glyphWidth = charInfo.width;
            characterInfo.glyphHeight = -charInfo.height;
            characterInfo.advance = charInfo.advance;

            characterInfos[i] = characterInfo;
        }
        customFont.characterInfo = characterInfos;

        string txtPath = filePath.Replace(fullFileName,txtName);
        Shader shader = Shader.Find("GUI/Text Shader");
        Material material = new Material(shader);
        Texture texture = AssetDatabase.LoadAssetAtPath<Texture>(txtPath);
        material.SetTexture("_MainTex",texture);
        AssetDatabase.CreateAsset(material, fileDir + fileName + ".mat");
        customFont.material = material;
        AssetDatabase.CreateAsset(customFont, fileDir + fileName + ".fontsettings");
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

    }

    private static List<CharInfo> GetCharInfo(byte[] textBytes,ref FontTxt fontTxt,ref string txtName)
    {
        if ( textBytes == null )
            return null;
        List<CharInfo> charInfos = new List<CharInfo>();

        ByteReader reader = new ByteReader( textBytes );
        char[] separater = new char[] { ' '};
        while (reader.canRead)
        {
            string line = reader.ReadLine();
            if (string.IsNullOrEmpty(line))
                break;
            string[] splits = line.Split( separater , StringSplitOptions.RemoveEmptyEntries );
            int len = splits.Length;

            if (splits[0].Equals("char"))
            {
                int chnl = len > 10 ? GetInt(splits[10]) : 15;
                if (len > 9 && GetInt(splits[9]) > 0)
                {
                    Debug.LogError("Your font was exported with more than one texture. Only one texture is supported by ArtistFont.\n" +
                                   "You need to re-export your font, enlarging the texture's dimensions until everything fits into just one texture.");
                    break;
                }
                if (len > 8)
                {
                    CharInfo charInfo = new CharInfo();
                    charInfo.index = GetInt(splits[1]);
                    charInfo.x = GetInt(splits[2]);
                    charInfo.y = GetInt(splits[3]);
                    charInfo.width = GetInt(splits[4]);
                    charInfo.height = GetInt(splits[5]);
                    charInfo.offsetX = GetInt(splits[6]);
                    charInfo.offsetY = GetInt(splits[7]);
                    charInfo.advance = GetInt(splits[8]);
                    charInfo.channel = chnl;
                    charInfos.Add(charInfo);
                }
                else
                {
                    Debug.LogError("Unexpected number of entries for the 'char' field with " + splits.Length + "\n" + line);
                    break;
                }
            }
            else if (splits[0] == "common")
            {
                // Expected data style:
                // common lineHeight=64 base=51 scaleW=512 scaleH=512 pages=1 packed=0 alphaChnl=1 redChnl=4 greenChnl=4 blueChnl=4

                if (len > 5)
                {
                    fontTxt.charSize = GetInt(splits[1]);
                    fontTxt.baseOffset = GetInt(splits[2]);
                    fontTxt.texWidth = GetInt(splits[3]);
                    fontTxt.texHeight = GetInt(splits[4]);

                    int pages = GetInt(splits[5]);

                    if (pages != 1)
                    {
                        Debug.LogError("Artist Font must be created with only 1 texture, not " + pages);
                        break;
                    }
                }
                else
                {
                    Debug.LogError("Unexpected number of entries for the 'common' field:\n" + line);
                    break;
                }
            }
            else if (splits[0] == "page")
            {
                // Expected data style:
                // page id=0 file="textureName.png"

                if (len > 2)
                {
                    txtName = GetString(splits[2]).Replace("\"", "");
                }
            }
        }

        return charInfos;
    }

    static string GetString( string s )
    {
        int index = s.IndexOf( '=' );
        return index == -1 ? "" : s.Substring( index + 1 );
    }

    static int GetInt( string s )
    {
        int val = 0;
        
        string valStr = GetString(s);
        try
        {
            val = int.Parse(valStr);
        }
        catch( System.Exception )
        {
            Debug.Log("there exist some error with parse string to int ");
        }
        return val;
    }
}

public struct CharInfo
{
    public int index;   // Index of this glyph (used by BMFont)
    public int x;       // Offset from the left side of the texture to the left side of the glyph
    public int y;       // Offset from the top of the texture to the top of the glyph
    public int width;   // Glyph's width in pixels
    public int height;  // Glyph's height in pixels
    public int offsetX; // Offset to apply to the cursor's left position before drawing this glyph
    public int offsetY; // Offset to apply to the cursor's top position before drawing this glyph
    public int advance; // How much to move the cursor after printing this character
    public int channel;	// Channel mask (in most cases this will be 15 (RGBA, 1+2+4+8)
}

public struct FontTxt
{
    public int charSize;
    public int baseOffset;
    public int texWidth;
    public int texHeight;
}

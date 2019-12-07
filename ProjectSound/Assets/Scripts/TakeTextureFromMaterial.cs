using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class TakeTextureFromMaterial : MonoBehaviour
{
    public Material[] materials;
    private void Start()
    {
        saveTexturesFromMaterials();
    }

    public void saveTexturesFromMaterials()
    {

        
        List<Texture2D> textures = new List<Texture2D>() ;
        foreach(Material mat in materials)
        {
            
            Color[] col = new Color[64];
            for(int j = 0; j < 64; j++)
            {
                col[j] = mat.color;
            }
            
            Texture2D tex = new Texture2D(8, 8);
            tex.SetPixels(col);
            
            tex.Apply();
            textures.Add(tex);
        }
        List<byte[]> bytes = new List<byte[]>();
        foreach(Texture2D tex in textures)
        {
            // Encode texture into PNG
            bytes.Add(tex.EncodeToPNG());
            
        }
        
        
        for(int i = 0; i < bytes.Count; i++)
        {
            
            // For testing purposes, also write to a file in the project folder
            File.WriteAllBytes(Application.dataPath + "/../Assets/Textures/"+ materials[i].name+ ".png", bytes[i]);
            i++;
        }
        
    }
}

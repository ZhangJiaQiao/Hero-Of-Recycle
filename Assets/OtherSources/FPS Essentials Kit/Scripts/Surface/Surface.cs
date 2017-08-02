using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// List of surfaces.
/// </summary>
public enum SurfaceType
{
    Default,
    Blood,
    Concrete,
    Dirt,
    Glass,
    Metal,
    Water,
    Wood
}

/// <summary>
/// Class responsible for grouping each texture to a surface type.
/// </summary>
[System.Serializable]
public class SurfaceList
{
    public SurfaceType surface = SurfaceType.Default; // Surface type of the texture.
    public Texture texture; // The texture.

    public SurfaceList (Texture texture)
    {
        this.texture = texture;
    }

    public Texture GetTexture ()
    {
        return texture;
    }

    public SurfaceType GetSurface ()
    {
        return surface;
    }
}

/// <summary>
/// Class responsible for managing the textures of an object and assimilating them to a surface type.
/// </summary>
public class Surface : MonoBehaviour 
{
    // List of textures in object.
    public SurfaceList[] surface = null;

    /// <summary>
    /// Returns the active terrain at the moment, make sure there is no more than one terrain in the scene.
    /// </summary>
    public Terrain terrain
    {
        get
        {
            return Terrain.activeTerrain;
        }
    }

    /// <summary>
    /// Returns the type of surface at the given position on the terrain.
    /// </summary>
    public SurfaceType GetSurface (Vector3 position, GameObject obj)
    {
        if (surface != null)
        {
            if (obj.GetComponent<Terrain>() != null)
            {
                return surface[SurfaceHelper.GetMainTexture(position, terrain.transform.position, terrain.terrainData)].GetSurface();
            }
            else
            {
                return surface[0].GetSurface();
            }
        }
        else
        {
            return SurfaceType.Default;
        }
    }

    /// <summary>
    /// Returns all textures applied to the object, be it an ordinary mesh or a terrain.
    /// </summary>
    public SurfaceList[] SetSurfaceList ()
    {
        List<SurfaceList> surfaces = new List<SurfaceList>();

        // Is the script coupled to a terrain?
        if (GetComponent<Terrain>() != null)
        {
            SplatPrototype[] splatPrototypes = terrain.terrainData.splatPrototypes;

            for (int i = 0; i < splatPrototypes.Length; i++)
            {
                surfaces.Add(new SurfaceList(splatPrototypes[i].texture));
            }
            return surfaces.ToArray();
        }
        else
        {
            Renderer renderer = gameObject.GetComponent<Renderer>();

            if (renderer != null)
            {
                surfaces.Add(new SurfaceList(renderer.sharedMaterial.mainTexture));
                return surfaces.ToArray();
            }
            else
            {
                surfaces.Add(new SurfaceList(CreateEmptyTexture(64,64)));
                return surfaces.ToArray();
            }
        }
    }

    /// <summary>
    /// Creates a default texture to indicate that there is no texture in the object.
    /// </summary>
    private Texture2D CreateEmptyTexture (int sizeX, int sizeY)
    {
        Texture2D tex = new Texture2D(sizeX, sizeY, TextureFormat.ARGB32, false);

        for (int x = 0; x < sizeX; x++)
        {
            for (int y = 0; y < sizeY; y++)
            {
                if (x < sizeX /2)
                {
                    if (y < sizeY / 2)
                    {
                        tex.SetPixel(x, y, Color.white);
                    }
                    else
                    {
                        tex.SetPixel(x, y, Color.gray);
                    }
                    
                }
                else
                {
                    if (y < sizeY / 2)
                    {
                        tex.SetPixel(x, y, Color.gray);
                    }
                    else
                    {
                        tex.SetPixel(x, y, Color.white);
                    }
                }
                
            }
        }
        tex.Apply();
        return tex;
    }
}
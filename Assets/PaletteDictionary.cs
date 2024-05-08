using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class PaletteDictionary : MonoBehaviour
{
    public PaletteDictionary instance;
    
    public enum thingColor
    {
        background,
        borderingTileAvailable,
        borderingTileUnavailable,
        ownedTileGrove,
        ownedTileForest,
        ownedTileGrassland
    }

    public Dictionary<thingColor, Color> PaletteDictionary_ = new Dictionary<thingColor, Color>();
    public PaletteItem[] paletteItems;
    
    [System.Serializable]
    public class PaletteItem
    {
        public thingColor thingName;
        public Color color;
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        
        foreach (PaletteItem paletteItem in paletteItems)
        {
            PaletteDictionary_.Add(paletteItem.thingName, paletteItem.color);
        }
    }
    
    
}

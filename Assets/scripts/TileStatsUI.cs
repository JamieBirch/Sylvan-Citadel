using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class TileStatsUI : MonoBehaviour
{
    [FormerlySerializedAs("Name")] public Text TileName;
    public GameObject tileResources;
    public GameObject tileBuildings;
    public GameObject tileStatPrefab;
    public Dictionary<string, TileStat> tileStatistics = new Dictionary<string, TileStat>();
    public Dictionary<string, TileStat> tileBuildingsDic = new Dictionary<string, TileStat>();

    public void AddStatField(string field, int count)
    {
        GameObject tileStatGO = Instantiate(tileStatPrefab, tileResources.transform);
        TileStat tileStat = tileStatGO.GetComponent<TileStat>();
        tileStat.SetName(field);
        tileStat.SetCount(count);
        tileStatistics.Add(field, tileStat);
    }
    
    public void AddBuildingField(string field)
    {
        if (tileBuildingsDic.TryGetValue(field, out TileStat tileStat))
        {
            tileStat.Increase();
        }
        else
        {
            GameObject tileStatGO = Instantiate(tileStatPrefab, tileBuildings.transform);
            TileStat newtileStat = tileStatGO.GetComponent<TileStat>();
            newtileStat.SetName(field);
            newtileStat.Increase();
            tileBuildingsDic.Add(field, newtileStat);
        }
    }

    public void UpdateFieldUi(string field, int count)
    {
        tileStatistics[field].SetCount(count);
    }

}

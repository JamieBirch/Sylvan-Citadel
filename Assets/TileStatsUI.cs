using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TileStatsUI : MonoBehaviour
{
    public Text Name;
    public GameObject tileStatPrefab;
    public Dictionary<string, TileStat> tileStatistics = new Dictionary<string, TileStat>();

    public void AddField(string field, int count)
    {
        GameObject tileStatGO = Instantiate(tileStatPrefab, gameObject.transform);
        TileStat tileStat = tileStatGO.GetComponent<TileStat>();
        tileStat.SetName(field);
        tileStat.SetCount(count);
    }

}

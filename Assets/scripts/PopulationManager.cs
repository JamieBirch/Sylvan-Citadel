using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Random = System.Random;

public class PopulationManager : MonoBehaviour
{
    public static PopulationManager instance;
    
    public GameObject village;
    public GameObject human;
    
    private Random rnd = new Random();
    public int maxPopulation;

    private void Awake()
    {
        instance = this;
    }
    
    public void SpawnHumans(int humansCount, GameObject hex)
    {
        var _village = CreateVillage(hex);
        
        for (int i = 0; i < humansCount; i++)
        {
            SpawnHuman(_village);
        }
    }

    public GameObject CreateVillage(GameObject hex)
    {
        //TODO check if village already exists
        
        GameObject _village = Instantiate(village, hex.transform.position, Quaternion.identity, hex.transform);
        _village.name = "village";
        hex.GetComponent<OwnedTile>().village = _village;
        return _village;
    }

    public GameObject SpawnHuman(GameObject village)
    {
        //create
        var position = TileUtils.PositionOnTile(village.transform.position);
        GameObject humanGameObject = Instantiate(human, position, Quaternion.identity, village.transform);
        GameStats.instance.AddHuman();

        //name
        string name = NameGenerator.CreateHumanName();
        humanGameObject.name = name;
        Human humanComponent = humanGameObject.GetComponent<Human>();
        humanComponent.Name = name;
        humanComponent.RandomizeAppearance();
        
        //settle
        OwnedTile homeTile = village.GetComponentInParent<OwnedTile>();
        Village villageComponent = village.GetComponent<Village>();
        SettleHumanInTile(homeTile, villageComponent, humanComponent);

        return humanGameObject;
    }
    
    public void SettleHumanInTile(OwnedTile newHomeTile, Village newVillage, Human humanComponent)
    {
        if (humanComponent.homeTile != null)
        {
            // move out of hex  
            Village homeVillage = humanComponent.homeTile.gameObject.GetComponentInChildren<Village>();
            homeVillage.humans.Remove(humanComponent);
            humanComponent.homeTile.HexPopulation--;
            
            //TODO !!!
            //move out of house
            /*if (humanComponent._home != null)
            {
                humanComponent._home.GetComponent<House>().MoveOut(humanComponent);
            }*/

            
            // GameObject newHomeVillage = newHomeHex.gameObject.GetComponentInChildren<Village>().gameObject;


            // humanComponent.isRelocating = true;
        }
        
        //move in to new Village
        humanComponent.gameObject.transform.SetParent(newVillage.transform.transform);
        newVillage.humans.Add(humanComponent);
        
        //move in to new hex
        humanComponent.homeTile = newHomeTile;
        // Debug.Log(humanComponent.name + " is settling in " + newHomeHex.Name);
        newHomeTile.HexPopulation++;
    }

    public List<Human> AllAvailableHumans(List<OwnedTile> ownedHexesAround)
    {
        List<Human> allAvailableHumans = new List<Human>();
        foreach (OwnedTile ownedHex in ownedHexesAround)
        {
            //should leave at least 1 human in each Hex
            int ownedHexAvailablePopulation = ownedHex.GetSettlersAvailable();
            GameObject ownedHexVillage = ownedHex.village;
            if (ownedHexVillage != null)
            {
                List<Human> hexHumans = ownedHexVillage.GetComponent<Village>().humans;
                //pick humans randomly
                var pickedHumansFromHex = hexHumans.OrderBy(x => rnd.Next()).Take(ownedHexAvailablePopulation);
                allAvailableHumans.AddRange(pickedHumansFromHex);
            }
        }

        return allAvailableHumans;
    }

}

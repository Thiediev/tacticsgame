using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitCarryover : MonoBehaviour {

    public static UnitCarryover instance;
    public List<Player> survivingUnits = new List<Player>();


    void Start () {
		
	}
	
	void Update () {
		
	}

    // When you clear a level, your surviving units have their data stored and can be brought back in later levels
    public void StoreUnits ()
    {
        foreach (Player u in GameManager.instance.players)
        {
            survivingUnits.Add(u);
            /*
            FleaPlusA playerFleaUpA;

            playerFleaUpA = ((GameObject)Instantiate(FleaUpAPrefab, new Vector3(attacker.transform.position.x, 0.55f, attacker.transform.position.z), Quaternion.Euler(new Vector3(0, 0, 180)))).GetComponent<FleaPlusA>();

            playerFleaUpA.gridPosition = new Vector2(attacker.transform.position.x + Mathf.Floor(mapSize / 2), -attacker.transform.position.z + Mathf.Floor(mapSize / 2));
            playerFleaUpA.playerName = "Juicenjam";

            playerFleaUpA.isOwnedByPlayerOne = true;

            players.Add(playerFleaUpA);
            playerOneCount++;

            FleaPlusA.Promotion(playerFleaUpA, attacker.HP);*/
        }
    }

    public void DisplayPocket ()
    {

        foreach (Player u in survivingUnits)
        {
            u.transform.position = new Vector3(0, 0, 0);
        }
    }
}

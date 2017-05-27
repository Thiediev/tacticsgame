using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.SceneManagement;

public class NWMapGeneration : NetworkBehaviour {
    /*
    // still need to relocate some stuff from NWGameManager here.

    // for now this is used to generate units properly.
    // this isn't really that important for online versus though, except for HQs. so, it can kind of wait, though HQs are important.




    [Command]
    public void CmdSpawnInfantry()
    {

        Debug.Log("ummmm this is happening");
        // create server-side instance
        GameObject fleaObject = (GameObject)Instantiate(NWGameManager.instance.FleaPrefab, transform.position, Quaternion.identity);
        // setup bullet component
        nwFlea1 = fleaObject.GetComponent<NWFlea>();
        nwFlea1.gridPosition = new Vector2(transform.position.x + Mathf.Floor(NWGameManager.instance.mapSize / 2), -transform.position.z + Mathf.Floor(NWGameManager.instance.mapSize / 2));

        // spawn on the clients
        NetworkServer.Spawn(fleaObject);

        // adding 'bullet' to the list of units      
        AddToList(fleaObject);

        RpcAddToList(fleaObject);
    }


    [ClientRpc]
    void RpcAddToList(GameObject obj)
    {
        // this code is executed on all clients
        //GameManager.instance.unitObjects.Add(obj);
        NWUnitList.instance.AddUnit(obj.GetComponent<NWUnit>());
    }


    public void AddToList(GameObject obj)
    {
        //if (isLocalPlayer && obj.GetComponent<NetworkIdentity>() != null)
        {
            CmdAddToList(obj);
        }
    }

    [Command]
    void CmdAddToList(GameObject obj)
    {
        // this code is only executed on the server
        RpcAddToList(obj); // invoke Rpc on all clients
    }
    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
    */
}

using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.Collections.Generic;
using System.Linq;


public class NWUnitList : NetworkBehaviour
{

    // I don't know if this is even necessary but hey


    public static NWUnitList instance;

    public List<NWUnit> unitList = new List<NWUnit>();

    public void Awake()
    {

        // this awake and start stuff is to make it so the static instance is initialized
        if (instance == null)
        {
            instance = this;
        }
    }

    public void Start()
    {
        NWGameManager.instance.RegisterAsUnitList(this);
    }

    public void AddUnit(NWUnit unitToAdd)
    {
        this.unitList.Add(unitToAdd);

        // AddUnit doesn't seem to be being called when it should be
        print("add unit please.");
        //unitList.Count();
    }

    // Update is called once per frame
    void Update()
    {
        NWGameManager.instance.RegisterAsUnitList(this);

    }
}

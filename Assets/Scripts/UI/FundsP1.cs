using UnityEngine;
using System.Collections;
using UnityEngine.UI;

using UnityEngine.SceneManagement;

public class FundsP1 : MonoBehaviour {

    public int fundoRundo = GameManager.instance.fundsArmyOne;
   // private Text P1Text;

    private Text P1Text;


    private void Awake()
    {
        P1Text = this.GetComponent<Text>();

    }
    // Use this for initialization
    void Start () {

    }

    // Update is called once per frame
    void Update () {
        UpdateFunds();

    }

    public void UpdateFunds()
    {
        fundoRundo = GameManager.instance.fundsArmyOne;
        P1Text.text = "P1: $" + fundoRundo.ToString();

    }
}

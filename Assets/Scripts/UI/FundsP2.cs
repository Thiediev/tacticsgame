using UnityEngine;
using System.Collections;
using UnityEngine.UI;

using UnityEngine.SceneManagement;

public class FundsP2 : MonoBehaviour
{

    public int p2FundsDisplay = GameManager.instance.fundsArmyTwo;
    // private Text P1Text;

    private Text P2Text;


    private void Awake()
    {
        P2Text = this.GetComponent<Text>();

    }
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        UpdateFunds();

    }

    public void UpdateFunds()
    {
        p2FundsDisplay = GameManager.instance.fundsArmyTwo;
        P2Text.text = "P2: $" + p2FundsDisplay.ToString();

    }
}

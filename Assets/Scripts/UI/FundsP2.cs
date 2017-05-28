using UnityEngine;
using System.Collections;
using UnityEngine.UI;

using UnityEngine.SceneManagement;

public class FundsP2 : MonoBehaviour
{
    public int p2FundsDisplay = GameManager.instance.fundsArmyTwo;
    private Text P2Text;

    private void Awake()
    {
        P2Text = this.GetComponent<Text>();
    }

    void Start()
    {

    }

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

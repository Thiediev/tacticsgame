using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class ChangeScene : MonoBehaviour {

    public ChangeScene instance;
    
	public void LoadCampaign()
    {
        SceneManager.LoadScene("Campaign");
    }
    public void About()
    {
        SceneManager.LoadScene("AboutScreen");
    }
    public void HowToPlay()
    {
        SceneManager.LoadScene("HowToPlay");
    }
    public void TestTime()
    {
        print("something");
    }
}

using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class ChangeScene : MonoBehaviour {

    public ChangeScene instance;
    
	public void LoadCampaign()
    {
        SceneManager.LoadScene("Campaign");
    }

    public void TestTime()
    {
        print("something");
    }
}

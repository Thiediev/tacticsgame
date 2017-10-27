using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGTiling : MonoBehaviour {

    public int offset = 1;

    //checking if we need to instantiate a new background tile
    public bool hasARightBuddy = false;
    public bool hasALeftBuddy = false;
    public bool hasATopBuddy = false;
    public bool hasABottomBuddy = false;

    public bool reverseScale = false; 

    private float spriteWidth = 0f;
    private float spriteHeight = 0f;

    private Transform myTransform;
    private Camera cam;

    void Awake ()
    {
        cam = Camera.main;
        myTransform = transform;
    }

    // Use this for initialization
    void Start () {
        SpriteRenderer sRenderer = GetComponent<SpriteRenderer>();
        spriteWidth = sRenderer.sprite.bounds.size.x;
        spriteHeight = sRenderer.sprite.bounds.size.y;
	}
	
	// Update is called once per frame
	void Update () {
        if (hasALeftBuddy == false || hasARightBuddy == false || hasATopBuddy == false || hasABottomBuddy == false)
        {
            float camHorizontalExtend = cam.orthographicSize * Screen.width / Screen.height;
            float camVerticalExtend = cam.orthographicSize * Screen.height / Screen.width;

            float edgeVisiblePositionRight = (myTransform.position.x + spriteWidth / 2) - camHorizontalExtend;
            float edgeVisiblePositionLeft = (myTransform.position.x - spriteWidth / 2) + camHorizontalExtend;

            //these might be flipped and i'm too hungry to figure it out atm
            float edgeVisiblePositionTop = (myTransform.position.z + spriteHeight / 2) - camVerticalExtend;
            float edgeVisiblePositionBottom = (myTransform.position.z - spriteHeight / 2) + camVerticalExtend;
         
             
            if (cam.transform.position.x >= edgeVisiblePositionRight - offset && hasARightBuddy == false)
            {
                MakeHorizontalBuddy(1);
                hasARightBuddy = true;
            }
            else if (cam.transform.position.x <= edgeVisiblePositionLeft + offset && hasALeftBuddy == false)
            {
                MakeHorizontalBuddy(-1);
                hasALeftBuddy = true;
            }
            
            
            if (cam.transform.position.z >= edgeVisiblePositionTop - offset && hasATopBuddy == false)
            {
                MakeVerticalBuddy(1);
                hasATopBuddy = true;
            }
            else if (cam.transform.position.z <= edgeVisiblePositionBottom + offset && hasABottomBuddy == false)
            {
                MakeVerticalBuddy(-1);
                hasABottomBuddy = true;
            }
        }

        /*
        if (hasATopBuddy == false || hasABottomBuddy == false)
        {
            float camVerticalExtend = cam.orthographicSize * Screen.height / Screen.width;
        }*/
    }

    void MakeHorizontalBuddy (int rightOrLeft)
    {
        Vector3 newPosition = new Vector3(myTransform.position.x + spriteWidth * rightOrLeft, myTransform.position.y, myTransform.position.z);
        Transform newBuddy = Instantiate(myTransform, newPosition, myTransform.rotation) as Transform;

        if (reverseScale == true)
            newBuddy.localScale = new Vector3(newBuddy.localScale.x * -1, newBuddy.localScale.y, newBuddy.localScale.z);

        newBuddy.parent = myTransform.parent;

        if (rightOrLeft > 0)
            newBuddy.GetComponent<BGTiling>().hasALeftBuddy = true;
        else
            newBuddy.GetComponent<BGTiling>().hasARightBuddy = true;
        
        }

    void MakeVerticalBuddy(int upOrDown)
    {
        Vector3 newPosition = new Vector3(myTransform.position.x, myTransform.position.y, myTransform.position.z + spriteHeight * upOrDown);
        Transform newBuddy = Instantiate(myTransform, newPosition, myTransform.rotation) as Transform;

        if (reverseScale == true)
            newBuddy.localScale = new Vector3(newBuddy.localScale.x, newBuddy.localScale.y, newBuddy.localScale.z * -1);

        newBuddy.parent = myTransform.parent;

        if (upOrDown > 0)
            newBuddy.GetComponent<BGTiling>().hasABottomBuddy = true;
        else
            newBuddy.GetComponent<BGTiling>().hasATopBuddy = true;

    }
}


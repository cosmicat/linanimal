using UnityEngine;
using System.Collections;

public class TwoLinanimal : MonoBehaviour {

    public GameObject mLeft;
    public GameObject mRight;
	// Use this for initialization
	void Start () {
        mLeft.AddComponent<InflatableMonoBehaviour>().inflateKey = KeyCode.Z;
        mRight.AddComponent<InflatableMonoBehaviour>().inflateKey = KeyCode.X;
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            mLeft.GetComponent<InflatableMonoBehaviour>().inflate();
        }

        if (Input.GetKeyUp(KeyCode.Z))
        {
            mLeft.GetComponent<InflatableMonoBehaviour>().inflate(false);
        }

        if (Input.GetKeyDown(KeyCode.X))
        {
            mRight.GetComponent<InflatableMonoBehaviour>().inflate();
        }

        if (Input.GetKeyUp(KeyCode.X))
        {
            mRight.GetComponent<InflatableMonoBehaviour>().inflate(false);
        }

	}
}

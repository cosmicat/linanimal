using UnityEngine;
using System.Collections;

public class NLinanimal : MonoBehaviour {
    public GameObject[] mAnimals;
    KeyCode[] mKeys = { KeyCode.Z, KeyCode.X, KeyCode.C, KeyCode.V, KeyCode.B, KeyCode.N, KeyCode.M };

    // Use this for initialization
    void Start()
    {
        for (int i = 0; i < mAnimals.Length; i++)
        {
            mAnimals[i].AddComponent<InflatableMonoBehaviour>().inflateKey = mKeys[i];
        }
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < mAnimals.Length; i++)
        {
            InflatableMonoBehaviour inflate = mAnimals[i].GetComponent<InflatableMonoBehaviour>();
            if (Input.GetKeyDown(inflate.inflateKey))
                inflate.inflate(true);
            if (Input.GetKeyUp(inflate.inflateKey))
                inflate.inflate(false);
        }
    }
}

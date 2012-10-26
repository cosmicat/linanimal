using UnityEngine;
using System.Collections;

public class FootScript : MonoBehaviour {

    SphereCollider mCollider;
    float radiusFactor;
    Vector3 mBaseScale;
    public KeyCode inflateKey;
    Vector3 mLastPosition = Vector3.zero;
    Vector3 mLastDirection = Vector3.zero;

    bool inflating;
    float inflateState = 0;

	// Use this for initialization
	void Start () 
    {
        //gameObject.AddComponent<Rigidbody>();
        mCollider = GetComponent<SphereCollider>();
        this.mBaseScale = transform.localScale;
        radiusFactor = 1.0f;
        inflating = false;
	}
	
	// Update is called once per frame
	void Update () 
    {
        if (Input.GetKeyDown(inflateKey))
        {
            this.inflate();
            this.inflating = true;
        }
        
        if (Input.GetKeyUp(inflateKey))
        {
            //this.deflate();
            this.inflating = false;
        }

        if (inflating)
        {
            inflateState += 5 * Time.deltaTime;
            inflateState = Mathf.Clamp01(inflateState + 3 * Time.deltaTime);
            this.inflate();
        }
        else
        {
            inflateState = Mathf.Clamp01(inflateState - Time.deltaTime);
        }

        transform.localScale = mBaseScale * (1 + 2*inflateState);

        Debug.DrawLine(mLastPosition, mLastPosition + mLastDirection * 100,Color.red);
	}

    void OnGUI()
    {
        if (Camera.current != null)
        {
            Vector3 pos = Camera.current.WorldToScreenPoint(this.transform.position);
            GUI.Label
                (
                new Rect(pos.x, Screen.height - pos.y, 50, 30),
                this.inflateKey.ToString()
                );
        }
    }

    void inflate()
    {
        //print(inflateKey + "inflate! ");
        //this.mCollider.radius = 2.5f * baseRadius;

        
        if (inflateState < 1)
        {
            Vector3 force = Vector3.zero;
            float divisor = 0;
            for (int i = 0; i < 50; i++)
            {
                Vector3 castDir = new Vector3(Mathf.Cos(i / 10.0f * Mathf.PI * 2), Mathf.Sin(i / 10.0f * Mathf.PI * 2), 0);
                Ray ray = new Ray(transform.position, castDir);
                RaycastHit hitInfo;
                if (Physics.Raycast(ray, out hitInfo, 1, 1 << 8))
                {
                    float coeff = 1 / (hitInfo.distance + 1);
                    divisor += coeff;
                    force += (-coeff * castDir);
                }
            }

            if (divisor > 0.0)
            {
                print("go!");
                force /= divisor;
                mLastDirection = force.normalized;
                mLastPosition = transform.position;
                collider.attachedRigidbody.AddForceAtPosition(3f * force, transform.position, ForceMode.Impulse);
            }
        }

    }

    void deflate()
    {
        inflating = false;
        //this.mCollider.radius = baseRadius;
    }
}

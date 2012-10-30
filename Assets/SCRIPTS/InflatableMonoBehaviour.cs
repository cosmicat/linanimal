using UnityEngine;
using System.Collections;

public class InflatableMonoBehaviour : MonoBehaviour
{

    SphereCollider mCollider;
    Vector3 mBaseScale;
    float mBaseColliderRadius;
    public KeyCode inflateKey;
    Vector3 mLastPosition = Vector3.zero;
    Vector3 mLastDirection = Vector3.zero;
    bool inflating;

    PhysicMaterial mPhysicsMaterial;

    AnimalTimer mInflatingTimer = new AnimalTimer(0, 1);

    // Use this for initialization
    void Start()
    {
        mCollider = GetComponent<SphereCollider>();
        this.mBaseScale = transform.localScale;
        mBaseColliderRadius = mCollider.radius;
        inflating = false;

    }

    public void inflate(bool aInflate = true)
    {
        inflating = aInflate;
    }
    float get_inflate_mutiplier()
    {
        return (1 + 1 * mInflatingTimer.getSquare());
    }
    float get_inflate_rate()
    {
        return mInflatingTimer.getLinear() * 2 * 8 * Time.deltaTime;
    }
    float get_absolute_radius()
    {
        Vector3 relScale = new Vector3(transform.localScale.x / mBaseScale.x, transform.localScale.y / mBaseScale.y, transform.localScale.z / mBaseScale.z);
        float[] asdf = { relScale.x, relScale.y, relScale.z };
        return Mathf.Max(asdf) * get_inflate_mutiplier()*0.5f;
    }
    // Update is called once per frame
    void Update()
    {
        if (inflating)
        {
            mInflatingTimer.update(8 * Time.deltaTime);
            this.apply_forces();
            if (mInflatingTimer.isExpired())
                inflating = false;
        }
        else
        {
            mInflatingTimer.update(-8*Time.deltaTime);
        }

        transform.localScale = mBaseScale * get_inflate_mutiplier();
        mCollider.radius = mBaseColliderRadius / get_inflate_mutiplier();// *get_inflate_mutiplier() * get_inflate_mutiplier();

        Debug.DrawLine(mLastPosition, mLastPosition + mLastDirection * 100, Color.red);
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

    void apply_forces()
    {
        //print(inflateKey + "inflate! ");
        //this.mCollider.radius = 2.5f * baseRadius;
        if (!mInflatingTimer.isExpired())
        {
            Vector3 force = Vector3.zero;
            float divisor = 0;
            for (int i = 0; i < 50; i++)
            {
                Vector3 castDir = new Vector3(Mathf.Cos(i / 10.0f * Mathf.PI * 2), Mathf.Sin(i / 10.0f * Mathf.PI * 2), 0);
                Ray ray = new Ray(transform.position, castDir);
                RaycastHit hitInfo;
                if (Physics.Raycast(ray, out hitInfo, get_absolute_radius()+0.1f, 1 << 8)) // todod radius thingy iss wrong
                {
                    float coeff = 1 / (hitInfo.distance + 1);
                    divisor += coeff;
                    force += (-coeff * castDir);
                }
            }

            if (divisor > 0.0)
            {
                force /= divisor;
                mLastDirection = force.normalized;
                mLastPosition = transform.position;
                collider.attachedRigidbody.AddForceAtPosition(60f * force, transform.position, ForceMode.Force);
            }
        }
    }

    void OnCollisionStay(Collision c)
    {
        /*
        foreach(ContactPoint e in c.contacts)
        {
            Vector3 np = e.point - get_inflate_rate()*e.normal + mCollider.attachedRigidbody.velocity * Time.deltaTime;
            Vector3 tp = Vector3.Exclude(e.normal, np - e.point) + e.point-np;
            mCollider.attachedRigidbody.AddForce(tp, ForceMode.VelocityChange);

        }*/
    }
    void OnCollisionEnter(Collision c)
    {

    }
}

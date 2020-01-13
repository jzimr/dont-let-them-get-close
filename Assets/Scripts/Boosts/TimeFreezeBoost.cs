using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeFreezeBoost : MonoBehaviour, IBaseBoost
{
    [SerializeField] float _radius = 10.0f;
    [SerializeField] float _freezeLength = 5.0f;

    public Vector3 startPosition { get; set; }
    public bool hasBeenUsed { get; set; }
    public string boostName { get; set; }

    void Start()
    {
        startPosition = transform.position;
        boostName = "Freeze Time Boost";
    }

    public void activateBoost()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, _radius);

        foreach(Collider collider in colliders)
        {
            if(collider.tag == "ParentGnome" || collider.tag == "Gnome")
                StartCoroutine(freezeObject(collider));
        }

        hasBeenUsed = true;
    }

    IEnumerator freezeObject(Collider collider)
    {
        if (collider.tag == "ParentGnome")
            collider.GetComponent<Rigidbody>().freezeRotation = true;
        else if (collider.tag == "Gnome")
            collider.GetComponent<GnomeBehaviour>().enabled = false;

        yield return new WaitForSeconds(_freezeLength);

        if (collider.tag == "ParentGnome")
            collider.GetComponent<Rigidbody>().freezeRotation = false;
        else if (collider.tag == "Gnome")
            collider.GetComponent<GnomeBehaviour>().enabled = true;
    }

    public void resetBoost()
    {
        transform.position = startPosition;
        hasBeenUsed = false;
        GetComponent<MeshRenderer>().enabled = true;
        GetComponent<Collider>().enabled = true;
    }
}

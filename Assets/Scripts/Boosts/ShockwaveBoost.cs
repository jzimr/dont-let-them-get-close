using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShockwaveBoost : MonoBehaviour, IBaseBoost
{
    [SerializeField] float _force = 5.0f;
    [SerializeField] float _radius = 5.0f;

    public Vector3 startPosition { get; set; }
    public bool hasBeenUsed { get; set; }
    public string boostName { get; set; }

    void Start()
    {
        startPosition = transform.position;
        boostName = "Shockwave boost";
    }

    void Update()
    {
    }

    /*
     * We activate the boost, effectively pushing all objects inside the radius away.
     */
    public void activateBoost()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, _radius);

        foreach(Collider collider in colliders)
        {
            Transform topParent = collider.transform.root;
            Rigidbody rb = topParent.GetComponent<Rigidbody>();

            if(rb != null)
            {
                Vector3 forceToAdd = Vector3.Normalize(topParent.transform.position - transform.position);
                forceToAdd.y += Random.Range(0.0f, _force);
                rb.AddForce(Vector3.Normalize(topParent.transform.position - transform.position) * _force);
            }
        }
        hasBeenUsed = true;
    }

    public void resetBoost()
    {
        transform.position = startPosition;
        hasBeenUsed = false;
        GetComponent<MeshRenderer>().enabled = true;
        GetComponent<Collider>().enabled = true;
    }
}

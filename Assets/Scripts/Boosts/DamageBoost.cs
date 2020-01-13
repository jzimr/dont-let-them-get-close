using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageBoost : MonoBehaviour, IBaseBoost
{
    [SerializeField] float _damage = 10.0f;
    [SerializeField] float _radius = 5.0f;

    public Vector3 startPosition { get; set; }
    public bool hasBeenUsed { get; set; }
    public string boostName { get; set; }

    // Start is called before the first frame update
    void Start()
    {
        startPosition = transform.position;
        boostName = "Damage boost";
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void activateBoost()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, _radius);

        foreach (Collider collider in colliders)
        {
            if(collider.tag == "ParentGnome")
            {
                Transform topParent = collider.transform.root;
                GnomeBehaviour gb = topParent.GetComponentInChildren<GnomeBehaviour>();
                Health health = topParent.GetComponent<Health>();

                if (health != null && gb != null)
                {
                    if (gb.getGnomeType() == GnomeBehaviour.GnomeType.Evil)
                    {
                        health.takeDamage(_damage);
                    }
                }
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

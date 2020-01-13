using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GnomeDetectBypasser : MonoBehaviour
{
    [SerializeField] GnomeBehaviour _gnomeBehaviour;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Gnome")
        {
            // they are my enemies
            if (other.GetComponent<GnomeBehaviour>().getGnomeType() != _gnomeBehaviour.getGnomeType())
            {
                // if gnome is not busy attacking
                if (!_gnomeBehaviour.isAttacking)
                {
                    // follow that gameobject and attack it
                    if (other.transform.parent != null)
                    {
                        _gnomeBehaviour.setFollow(other.transform.parent.gameObject);
                    }
                    else
                    {
                        _gnomeBehaviour.setFollow(other.gameObject);
                    }
                }
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {

    }
}

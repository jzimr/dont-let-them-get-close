using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoostManager : MonoBehaviour
{
    [SerializeField] float minRespawnTime = 20.0f;
    [SerializeField] float maxRespawnTime = 40.0f;

    private GameObject[] _boosts;
     
    // Start is called before the first frame update
    void Start()
    {
        // find all objects containing the "Boost" tag
        _boosts = GameObject.FindGameObjectsWithTag("Boost");

        // check every 0.5 seconds if a boost is disabled and should be enabled some time in the future
        InvokeRepeating("checkForRespawns", 0.0f, 1.0f);
    }

    /*
     * Check if a boost has been used by player and needs to be respawned
     */
    void checkForRespawns()
    {
        foreach(GameObject boost in _boosts){
            if (boost.GetComponent<IBaseBoost>().hasBeenUsed)
            {
                StartCoroutine(respawnBoost(boost));
            }
        }
    }

    /*
     * Respawn the boost to its initial state
     */
    IEnumerator respawnBoost(GameObject boostToRespawn)
    {
        yield return new WaitForSeconds(Random.Range(minRespawnTime, maxRespawnTime));
        boostToRespawn.GetComponent<IBaseBoost>().resetBoost();
    }

    // Update is called once per frame
    void Update()
    {
    }
}

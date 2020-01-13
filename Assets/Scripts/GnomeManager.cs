using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GnomeManager : MonoBehaviour
{
    [SerializeField] private GameObject _evilGnomePrefab;
    [SerializeField] private GameObject _goodGnomePrefab;

    [SerializeField] private GameObject _evilCrystal;
    [SerializeField] private GameObject _goodCrystal;

    [SerializeField] private int _evilStartAmount;
    [SerializeField] private int _goodStartAmount;
    [SerializeField] private int _evilIncreaseAmount = 2;
    [SerializeField] private float _evilIncreaseInterval = 10.0f;
    [SerializeField] private int _respawnAmount = 10;
    [SerializeField] private float _respawnCooldown = 10.0f;

    private List<GameObject> _evilGnomes;
    private List<GameObject> _goodGnomes;
    private int respawnNumber = 0;

    // Start is called before the first frame update
    void Start()
    {
        _evilGnomes = new List<GameObject>();
        _goodGnomes = new List<GameObject>();

        // we generate pre-defined amount of gnomes on both teams and spawn them
        for(int i = 0; i < _evilStartAmount; i++)
        {
            GameObject newGnome = Instantiate(_evilGnomePrefab);
            newGnome.SetActive(false);
            _evilGnomes.Add(newGnome);
        }
        for (int i = 0; i < _goodStartAmount; i++)
        {
            GameObject newGnome = Instantiate(_goodGnomePrefab);
            newGnome.SetActive(false);
            _goodGnomes.Add(newGnome);
        }
        spawnGnomes(GnomeBehaviour.GnomeType.Evil, _evilStartAmount);
        spawnGnomes(GnomeBehaviour.GnomeType.Good, _goodStartAmount);

        // Increase enemy amount every x second
        InvokeRepeating("increaseGnomesCount", 0.0f, _evilIncreaseInterval);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void increaseGnomesCount()
    {
        for(int i = 0; i < _evilIncreaseAmount; i++)
        {
            GameObject newGnome = Instantiate(_evilGnomePrefab);
            newGnome.SetActive(false);
            _evilGnomes.Add(newGnome);
        }
        _evilStartAmount += _evilIncreaseAmount;

        spawnGnomes(GnomeBehaviour.GnomeType.Evil, _evilIncreaseAmount);
    }

    private void spawnGnomes(GnomeBehaviour.GnomeType gnomeType, int amount)
    {
        if (gnomeType == GnomeBehaviour.GnomeType.Evil)
        {
            for(int i = 0; i < amount; i++)
            {
                GameObject newGnome = _evilGnomes[i];

                newGnome.transform.position = new Vector3(
                    _evilCrystal.transform.position.x + (i % ((int)Mathf.Sqrt(amount))) - 5,
                    _evilCrystal.transform.position.y,
                    _evilCrystal.transform.position.z - 1 - (i / ((int)Mathf.Sqrt(amount)))
                );
                newGnome.SetActive(true);
            }
            _evilGnomes.RemoveRange(0, amount);
        }
        else if (gnomeType == GnomeBehaviour.GnomeType.Good)
        {
            //Debug.Log(amount + " | " + _goodGnomes.Count);
            for (int i = 0; i < amount; i++)
            {
                GameObject newGnome2 = _goodGnomes[i];

                newGnome2.transform.position = new Vector3(
                    _goodCrystal.transform.position.x + (i % ((int)Mathf.Sqrt(amount))) - 5,
                    _goodCrystal.transform.position.y,
                    _goodCrystal.transform.position.z + 1 + (i / ((int)Mathf.Sqrt(amount)))
                );
                newGnome2.SetActive(true);
            }
            _goodGnomes.RemoveRange(0, amount);
        }
    }

    public void removeGnome(GameObject gnome)
    {
        gnome.SetActive(false);
        gnome.GetComponent<Health>().reset();        // reset the health of the gnome
        GnomeBehaviour gb = gnome.GetComponentInChildren<GnomeBehaviour>();
        
        if(gb.getGnomeType() == GnomeBehaviour.GnomeType.Evil)
        {
            _evilGnomes.Add(gnome);

            if (_evilGnomes.Count % _respawnAmount == 0 || _evilGnomes.Count == 1)
            {
                StartCoroutine(WaitThenSpawnNewCoroutine(gb.getGnomeType()));
            }
        }
        else if (gb.getGnomeType() == GnomeBehaviour.GnomeType.Good)
        {
            _goodGnomes.Add(gnome);

            if (_goodGnomes.Count % _respawnAmount == 0 || _goodGnomes.Count == 1)
            {
                StartCoroutine(WaitThenSpawnNewCoroutine(gb.getGnomeType()));
            }
        }
    }

    private int evilQueueCounter = 0;
    private int evilQueueTurn = 0;
    private int goodQueueCounter = 0;
    private int goodQueueTurn = 0;
    IEnumerator WaitThenSpawnNewCoroutine(GnomeBehaviour.GnomeType gnomeType)
    {
        int placeInQueue = gnomeType == GnomeBehaviour.GnomeType.Evil ? evilQueueCounter++ : goodQueueCounter++;
        int queueTurn = gnomeType == GnomeBehaviour.GnomeType.Evil ? evilQueueTurn : goodQueueTurn;

        //Debug.Log(gnomeType + " placed in queue number " + placeInQueue + ". Current queue turn " + queueTurn);
        yield return new WaitForSeconds(_respawnCooldown);

        // wait until we have at least the minimum amount of gnomes in the pool and the minimum
        // respawn time has been met
        if(gnomeType == GnomeBehaviour.GnomeType.Evil)
        {
            while(_evilGnomes.Count < _respawnAmount
                || _evilGnomes[_respawnAmount - 1].GetComponentInChildren<GnomeBehaviour>()._timeOfDeath + _respawnCooldown > Time.time
                )
            {
                yield return new WaitForSeconds(0.3f);
            }
        }
        if (gnomeType == GnomeBehaviour.GnomeType.Good)
        {
            while (_goodGnomes.Count < _respawnAmount
                || _goodGnomes[_respawnAmount - 1].GetComponentInChildren<GnomeBehaviour>()._timeOfDeath + _respawnCooldown > Time.time
                )
            {
                yield return new WaitForSeconds(0.3f);
            }
        }


        // wait until it is our turn to spawn
        bool someOneWasBeforeUs = false;
        while (((gnomeType == GnomeBehaviour.GnomeType.Evil && _evilGnomes.Count <= _respawnAmount) ||
            (gnomeType == GnomeBehaviour.GnomeType.Good && _goodGnomes.Count <= _respawnAmount))
                || placeInQueue != queueTurn)
        {
            someOneWasBeforeUs = true;
            //Debug.Log(gnomeType + " can spawn, but someone is infront of us (" + placeInQueue + ", " + queueTurn + ")");
            yield return new WaitForSeconds(0.2f);
            queueTurn = gnomeType == GnomeBehaviour.GnomeType.Evil ? evilQueueTurn : goodQueueTurn;
        }

        // if there was someone before us, we wait and then spawn
        if (someOneWasBeforeUs)
        {
            //Debug.Log(gnomeType + " can spawn, but someone just spawned, so we wait " + _respawnCooldown.ToString() + "s");
            yield return new WaitForSeconds(_respawnCooldown);
        }

        //Debug.Log("Did respawn :D");
        spawnGnomes(gnomeType, _respawnAmount);

        if (gnomeType == GnomeBehaviour.GnomeType.Evil)
        {
            evilQueueTurn++;
            //Debug.Log(gnomeType + " queue is now at " + evilQueueTurn);
        }
        else
        {
            goodQueueTurn++;
            //Debug.Log(gnomeType + " queue is now at " + goodQueueTurn);
        }
    }
}

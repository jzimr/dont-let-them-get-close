using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CrystalBehaviour : MonoBehaviour
{
    enum TypeOfCrystal
    {
        Evil,
        Good
    }

    [SerializeField] private TypeOfCrystal _typeOfCrystal;
    [SerializeField] private Text gameStateMessage;
    [SerializeField] private GeneralSettings settings;

    private Health _health;

    // Start is called before the first frame update
    void Start()
    {
        _health = transform.GetComponent<Health>();
    }

    // Update is called once per frame
    void Update()
    {
        // we ded
        if (_health.getHealth() <= 0.0f)
        {
            crystalDied();
        }
    }

    void crystalDied()
    {
        settings.stopTimer();

        // we won!
        if(_typeOfCrystal == TypeOfCrystal.Evil)
        {
            gameStateMessage.text = "YOU WON!";
        }
        // we lost :(
        else
        {
            gameStateMessage.text = "YOU Lost :(";
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GeneralSettings : MonoBehaviour
{
    [SerializeField] private Text timer;

    private bool gameEnd = false;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!gameEnd)
        {
            float timePassed = Time.realtimeSinceStartup;
            int minute = (int)timePassed / 60;
            int second = (int)timePassed % 60;
            int millisecond = (int)((timePassed % 1) * 100);
            timer.text =
                (minute < 10 ? "0" + minute.ToString() : minute.ToString()) + ":" +
                (second < 10 ? "0" + second.ToString() : second.ToString()) + ":" +
                (millisecond < 10 ? "0" + millisecond.ToString() : millisecond.ToString());
        }                                   
    }

    public void stopTimer()
    {
        gameEnd = true;
    }
}

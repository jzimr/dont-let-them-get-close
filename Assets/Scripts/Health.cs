using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] private float _health;
    [SerializeField] private Renderer _damageIndicator;
    private Color damageColor;
    [SerializeField] private Color defaultColor;
    private Color currentHealthColor;

    private float _startHealth;
    private float lowestColour = 40.0f;
    private float colorToDecrease;

    void Start() {
        _startHealth = _health;
        _damageIndicator.material.SetColor("_Color", defaultColor);
        colorToDecrease = (250f - lowestColour) / _startHealth;
        damageColor = defaultColor / 8f;
        currentHealthColor = defaultColor;
    }

    public float getHealth()
    {
        return _health;
    }

    public void takeDamage(float damage)
    {
        _health -= damage;

        StartCoroutine(UIDamage());
        // todo add force to this object when hit
    }

    IEnumerator UIDamage()
    {
        _damageIndicator.material.color = damageColor;
        yield return new WaitForSeconds(0.1f);
        // show the decrease in health by changing defaultcolor

        if(defaultColor.r > defaultColor.g)
            currentHealthColor.r = (lowestColour + _health * colorToDecrease) / 255f;
        else
            currentHealthColor.g = (lowestColour + _health * colorToDecrease) / 255f;

        _damageIndicator.material.color = currentHealthColor;
    }

    public void increaseHealth(float health)
    {
        _health += health;
    }

    public void reset()
    {
        _health = _startHealth;
        _damageIndicator.material.color = defaultColor;
    }
}

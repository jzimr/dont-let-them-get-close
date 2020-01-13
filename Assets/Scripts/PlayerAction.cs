using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;

public class PlayerAction : MonoBehaviour
{
    [SerializeField] private float maxAttackDistance = 5.0f;
    [SerializeField] private float onHitRadius = 2.0f;
    [SerializeField] private float attackDamage = 2.0f;
    [SerializeField] private float attackCooldown = 2.0f;

    [SerializeField] private Color availableColor;
    [SerializeField] private Color unavailableColor;

    [SerializeField] private Text attackMessage;
    [SerializeField] private Text boostMessage;

    GameObject _currentBoost = null;
    private float _nextAttack = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        availableColor.a = 1;
        unavailableColor.a = 1;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if(_currentBoost != null)
            {
                // we drop the boost where player is atm
                _currentBoost.transform.position = new Vector3(
                    transform.position.x,
                    transform.position.y / 2,
                    transform.position.z);
                _currentBoost.GetComponent<IBaseBoost>().activateBoost();
                _currentBoost.GetComponent<MeshRenderer>().enabled = false;
                _currentBoost.GetComponent<Collider>().enabled = false;
                _currentBoost = null;
                changeText(boostMessage, unavailableColor, "No boost");
            }
        }

        if(Time.time > _nextAttack && attackMessage.color == unavailableColor)
        {
            changeText(attackMessage, availableColor, "Can Attack");
        }

        // player wants to attack gnome
        if (Input.GetMouseButton(0) && Time.time > _nextAttack)
        {
            RaycastHit hit;
            var cameraCenter = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width / 2f, Screen.height / 2f, Camera.main.nearClipPlane));
            Debug.DrawRay(cameraCenter, Camera.main.transform.forward * maxAttackDistance, Color.blue);

            if (Physics.Raycast(cameraCenter, Camera.main.transform.forward, out hit, maxAttackDistance))
            {
                GameObject lookingAt = hit.transform.gameObject;
                attack(lookingAt);
            }

            _nextAttack = Time.time + attackCooldown;
            changeText(attackMessage, unavailableColor, "Attack on cooldown");
        }
    }

    void attack(GameObject lookingAt)
    {
        // get all objects within a particular radius
        Collider[] colliders = Physics.OverlapSphere(lookingAt.transform.position, onHitRadius);

        foreach(Collider collider in colliders)
        {
            GameObject poorFella = collider.gameObject;

            // Only hit evil ones
            if(poorFella.tag.Equals("ParentGnome") &&
                poorFella.GetComponentInChildren<GnomeBehaviour>().getGnomeType() == GnomeBehaviour.GnomeType.Evil)
            {
                poorFella.GetComponentInChildren<Health>().takeDamage(attackDamage);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // search exclusively for boosts. We can only pickup if we have none available atm
        if(other.CompareTag("Boost"))
        {
            if(_currentBoost == null)
            {
                _currentBoost = other.gameObject;
                _currentBoost.GetComponent<MeshRenderer>().enabled = false;
                _currentBoost.GetComponent<Collider>().enabled = false;     // we make object invisible
                changeText(boostMessage, availableColor, _currentBoost.GetComponent<IBaseBoost>().boostName);
            }
        }
    }

    private void changeText(Text textItem, Color newColor, string newtext)
    {
        textItem.color = newColor;
        textItem.text = newtext;
    }
}
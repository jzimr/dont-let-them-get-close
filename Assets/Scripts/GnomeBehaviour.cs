using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GnomeBehaviour : MonoBehaviour
{
    public enum GnomeType
    {
        Evil,
        Good
    }

    [SerializeField] private GnomeType _gnomeType;
    [SerializeField] private GameObject _evilCrystal;
    [SerializeField] private GameObject _goodCrystal;

    [SerializeField] private GnomeManager _gnomeManager;

    [SerializeField] private float _movingSpeed = 3.0f;

    [SerializeField] private float _attackDamage = 2.0f;
    [SerializeField] private float _attackCooldown = 1.0f;
    [SerializeField] private float _maxAttackDistance = 5.0f;

    public bool isAttacking = false;
    public float _timeOfDeath = -100.0f;

    private Health _health;
    private float _nextAttack = 0.0f;
    private GameObject _crystalToAttack;
    private GameObject _movingTowards;

    // Start is called before the first frame update
    void Start()
    {
        _health = transform.GetComponentInParent<Health>();
        _crystalToAttack = _gnomeType == GnomeType.Good ? _evilCrystal : _goodCrystal;
        _movingTowards = _crystalToAttack;   // default: go towards crystal
    }

    // Update is called once per frame
    void Update()
    {
        // we ded
        if(_health.getHealth() <= 0.0f)
        {
            _gnomeManager.removeGnome(transform.parent.gameObject);
            _timeOfDeath = Time.time;
        }

        // gnome can only move if not attacking
        if(Time.time > _nextAttack)
        {
            isAttacking = false;

            // if we are not going toward anyone and 
            if(_movingTowards != null && !_movingTowards.activeSelf)
            {
                _movingTowards = _crystalToAttack;   // default: go towards crystal
            }

            // we move the parent
            Transform parent = transform.parent;

            parent.LookAt(_movingTowards.transform, Vector3.up);
            Vector3 moveDirection = Vector3.forward * _movingSpeed * Time.deltaTime;
            moveDirection.y = 0;
            parent.Translate(moveDirection);
        }
    }

    public GnomeType getGnomeType()
    {
        return _gnomeType;
    }

    private void OnTriggerEnter(Collider other)
    {
        doDamage(other);
    }

    private void OnTriggerStay(Collider other)
    {
        doDamage(other);
    }

    /*
     * Do damage to other entity if possible
     */
    private void doDamage(Collider other)
    {
        // if gnome is currently on cooldown
        if (Time.time < _nextAttack)
            return;


        if (other.gameObject.tag == "Gnome")
        {
            Health otherHealth = other.gameObject.GetComponentInParent<Health>();
            GnomeBehaviour otherGnome = other.gameObject.GetComponentInParent<GnomeBehaviour>();

            // if not on my team
            if (otherGnome._gnomeType != _gnomeType)
            {
                otherHealth.takeDamage(_attackDamage);
                _nextAttack = Time.time + _attackCooldown;
                isAttacking = true;
            }
        }
        else if (other.gameObject.tag == "Crystal")
        {
            if (other.gameObject.Equals(_crystalToAttack))
            {
                Health otherHealth = other.gameObject.GetComponentInParent<Health>();
                otherHealth.takeDamage(_attackDamage);
                _nextAttack = Time.time + _attackCooldown;
                isAttacking = true;
            }
        }
        else if(other.gameObject.tag == "Player")
        {
            // todo
        }
    }

    public void setFollow(GameObject toFollow)
    {
        _movingTowards = toFollow;
    }
}

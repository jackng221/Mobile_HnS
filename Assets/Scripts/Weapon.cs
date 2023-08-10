using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Weapon : MonoBehaviour
{
    [SerializeField] EquipmentData wepData;
    ICharacter character;
    List<Collider> hitCharacters;

    public EquipmentData WeaponData { get { return wepData; } }
    public event Action OnWeaponHit;
    public int currentComboOnCharacter { get; set; }

    private void Start()
    {
        hitCharacters = new List<Collider>();
        character = GetComponentInParent<ICharacter>();
        MonoBehaviour mb = character as MonoBehaviour;
        foreach(Collider collider in mb.gameObject.GetComponentsInChildren<Collider>())
        {
            Physics.IgnoreCollision(gameObject.GetComponent<Collider>(), collider);
        }
        EnableHitbox(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<ICharacter>() != null)
        {
            Physics.IgnoreCollision(gameObject.GetComponent<Collider>(), other);
            hitCharacters.Add(other);

            OnWeaponHit? .Invoke();
            other.GetComponent<ICharacter>().Damage( (character.attackPt + wepData.attackPt) * wepData.stanceData.comboMultiplier[currentComboOnCharacter] );
        }
    }

    public void EnableHitbox(bool boolean)
    {
        GetComponent<Collider>().enabled = boolean;
        if (boolean)
        {
            foreach (Collider other in hitCharacters)
            {
                if (other)
                Physics.IgnoreCollision(gameObject.GetComponent<Collider>(), other, false); //re-enable collision
            }
            hitCharacters.Clear();
        }
    }

    [ContextMenu("HitboxOn")]
    void HitboxOn()
    {
        EnableHitbox(true);
    }
}

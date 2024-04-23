using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamable {
    public void TakeDame(HitParam hitParam);
    public bool IsDead();
    public void Die();
    public void Heal(float healAmount);
}

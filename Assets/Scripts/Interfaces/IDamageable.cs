using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamageable 
{
   public int HP { get;  set; }

   public void TakeDamage(int damage);

   public void NoHP();
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IBreakable : IDamageable
{
   public bool IsBroken { get; set; }
   public GameObject BrokenObj { get; set; }
   public void Break();
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IBreakable 
{
   public bool IsBroken {  get; set; }

   public void Break();
}

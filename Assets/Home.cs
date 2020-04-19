using System;
using System.Collections;
using System.Collections.Generic;
using Entities.Player;
using UnityEngine;

public class Home : MonoBehaviour
{
   public GameObject DoubleJumpPlant;
   public GameObject VinePlant;
   public GameObject SlamPlant;

   private void Start()
   {
      UpdatePlants();
   }

   public void UpdatePlants()
   {
      var pc = FindObjectOfType<PlayerController>();
      if (pc == null)
         return;
      DoubleJumpPlant.SetActive(pc.CanDoubleJump);
      VinePlant.SetActive(pc.CanVine);
      SlamPlant.SetActive(pc.CanSlam);
   }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RibBoneController : BoneController
{
    new protected void Update()
    {
        if(currentState == "Grounded")
        {
            //create a rib cage that traps whatever is in it (obstacle for dogs and player)
            Destroy(gameObject);
        }
    }
}

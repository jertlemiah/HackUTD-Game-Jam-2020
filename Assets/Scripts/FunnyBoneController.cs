using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FunnyBoneController : BoneController
{
    new protected void Update()
    {
        base.Update();
        if(currentState == "Grounded")
        {
            //make a temporary aoe that acts as an obstacle for dogs
            Destroy(gameObject, 1f);
        }

    }
}

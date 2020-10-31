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
            GameObject cage = gameObject.GetComponentInChildren<GameObject>();
            cage.SetActive(true);
            Destroy(gameObject, 1f);
        }
    }
}

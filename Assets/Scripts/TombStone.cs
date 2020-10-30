using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TombStone : MonoBehaviour
{
    public GameObject[] bones = new GameObject[3];
    //0 is normal, 1 is funny, 2 is rib

    void Update()
    {
        //if player is nearby and interacts
            //give out 5 bones
            //50% chance normal, 25% for the other two (per each bone given, not as a group)
    }
}

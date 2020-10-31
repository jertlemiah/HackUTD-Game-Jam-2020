using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TombStone : MonoBehaviour
{

    bool opened = false;

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Player" && !opened)
        {
            PlayerMovement player = other.GetComponent<PlayerMovement>();
            for(int i = 0; i<5; i++)
            {
                int rand = Random.Range(1, 4);
                if(rand == 1 || rand == 2)
                {
                    player.normalBones++;
                }
                else if(rand == 3)
                {
                    player.funnyBones++;
                }
                else if(rand == 4)
                {
                    player.ribBones++;
                }
            }
            opened = true;
        }
    }
}

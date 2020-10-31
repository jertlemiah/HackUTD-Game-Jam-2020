using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Healthbar : MonoBehaviour
{

    public float MaxHealth;
    public float CurrentHealth;
    [SerializeField]
    public Sprite[] SpriteList;
    //public SpriteRenderer spriteRenderer;
    public Image image;

    private void Start()
    {
        MaxHealth = StateManager.Instance.playerObject.GetComponent<PlayerMovement>().MaxHealth;
    }

    private void Update()
    {
        CurrentHealth = StateManager.Instance.playerObject.GetComponent<PlayerMovement>().CurrentHealth;
        int layer = (int)((CurrentHealth / MaxHealth) * SpriteList.Length);

        if (layer < 0)
        {
            image.sprite = SpriteList[0];
            // also trigger death screen
        }
        else if (layer < SpriteList.Length)
        {
            image.sprite = SpriteList[layer];
        }
        else if (layer >= SpriteList.Length)
        {
            image.sprite = SpriteList[SpriteList.Length - 1];
        }
    }
}

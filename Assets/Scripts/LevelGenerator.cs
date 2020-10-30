using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


public class LevelGenerator : MonoBehaviour
{
    [SerializeField]
    protected GameObject[] startRooms;
    [SerializeField]
    protected GameObject[] keyRooms;
    [SerializeField]
    protected GameObject[] doorRooms;
    [SerializeField]
    protected GameObject[] pickupRooms;

    public Vector2 posBottomLeft = new Vector2(-16, -16);
    public Vector2 posBottomRight = new Vector2(16, -16);
    public Vector2 posTopLeft = new Vector2(-16, 16);
    public Vector2 posTopRight = new Vector2(16, 16);

    bool startRoom = false;
    bool keyRoom = false;
    bool doorRoom = false;
    bool pickupRoom = false;

    // Start is called before the first frame update
    void Start()
    {
        for(int i = 0; i<4; i++)
        {
            int rand = Random.Range(1, 4);
            if (i == 0) //bottom left selection
            {
                if (rand == 1)
                {
                    Instantiate(startRooms[Random.Range(0, startRooms.Length)], posBottomLeft, Quaternion.identity);
                    startRoom = true;
                }
                else if (rand == 2)
                {
                    Instantiate(keyRooms[Random.Range(0, keyRooms.Length)], posBottomLeft, Quaternion.identity);
                    keyRoom = true;
                }
                else if (rand == 3)
                {
                    Instantiate(doorRooms[Random.Range(0, doorRooms.Length)], posBottomLeft, Quaternion.identity);
                    doorRoom = true;
                }
                else if (rand == 4)
                {
                    Instantiate(pickupRooms[Random.Range(0, pickupRooms.Length)], posBottomLeft, Quaternion.identity);
                    pickupRoom = true;
                }
            }

            else if (i == 1) //bottom right selection
            {
                if (rand == 1 && !startRoom)
                {
                    Instantiate(startRooms[Random.Range(0, startRooms.Length)], posBottomRight, Quaternion.identity);
                    startRoom = true;
                }
                else if (rand == 2 && !keyRoom)
                {
                    Instantiate(keyRooms[Random.Range(0, keyRooms.Length)], posBottomRight, Quaternion.identity);
                    keyRoom = true;
                }
                else if (rand == 3 && !doorRoom)
                {
                    Instantiate(doorRooms[Random.Range(0, doorRooms.Length)], posBottomRight, Quaternion.identity);
                    doorRoom = true;
                }
                else if (rand == 4 && !pickupRoom)
                {
                    Instantiate(pickupRooms[Random.Range(0, pickupRooms.Length)], posBottomRight, Quaternion.identity);
                    pickupRoom = true;
                }
                else //room was chosen already
                {
                    rand = Random.Range(1, 3);
                    if (startRoom)
                    {
                        if (rand == 1 && !keyRoom)
                        {
                            Instantiate(keyRooms[Random.Range(0, keyRooms.Length)], posBottomRight, Quaternion.identity);
                            keyRoom = true;
                        }
                        else if (rand == 2 && !doorRoom)
                        {
                            Instantiate(doorRooms[Random.Range(0, doorRooms.Length)], posBottomRight, Quaternion.identity);
                            doorRoom = true;
                        }
                        else if (rand == 3 && !pickupRoom)
                        {
                            Instantiate(pickupRooms[Random.Range(0, pickupRooms.Length)], posBottomRight, Quaternion.identity);
                            pickupRoom = true;
                        }
                    }
                    else if (keyRoom)
                    {
                        if (rand == 1 && !startRoom)
                        {
                            Instantiate(startRooms[Random.Range(0, startRooms.Length)], posBottomRight, Quaternion.identity);
                            startRoom = true;
                        }
                        else if (rand == 2 && !doorRoom)
                        {
                            Instantiate(doorRooms[Random.Range(0, doorRooms.Length)], posBottomRight, Quaternion.identity);
                            doorRoom = true;
                        }
                        else if (rand == 3 && !pickupRoom)
                        {
                            Instantiate(pickupRooms[Random.Range(0, pickupRooms.Length)], posBottomRight, Quaternion.identity);
                            pickupRoom = true;
                        }
                    }
                    else if (doorRoom)
                    {
                        if (rand == 1 && !startRoom)
                        {
                            Instantiate(startRooms[Random.Range(0, startRooms.Length)], posBottomRight, Quaternion.identity);
                            startRoom = true;
                        }
                        else if (rand == 2 && !keyRoom)
                        {
                            Instantiate(doorRooms[Random.Range(0, keyRooms.Length)], posBottomRight, Quaternion.identity);
                            keyRoom = true;
                        }
                        else if (rand == 3 && !pickupRoom)
                        {
                            Instantiate(pickupRooms[Random.Range(0, pickupRooms.Length)], posBottomRight, Quaternion.identity);
                            pickupRoom = true;
                        }
                    }
                    else if (pickupRoom)
                    {
                        if (rand == 1 && !startRoom)
                        {
                            Instantiate(startRooms[Random.Range(0, startRooms.Length)], posBottomRight, Quaternion.identity);
                            startRoom = true;
                        }
                        else if (rand == 2 && !keyRoom)
                        {
                            Instantiate(doorRooms[Random.Range(0, keyRooms.Length)], posBottomRight, Quaternion.identity);
                            keyRoom = true;
                        }
                        else if (rand == 3 && !doorRoom)
                        {
                            Instantiate(doorRooms[Random.Range(0, doorRooms.Length)], posBottomRight, Quaternion.identity);
                            doorRoom = true;
                        }
                    }
                }
            }

            else if (i == 2) //top left selection
            {
                if (rand == 1)
                {
                    Instantiate(startRooms[Random.Range(0, startRooms.Length)], posTopLeft, Quaternion.identity);
                    startRoom = true;
                }
                else if (rand == 2)
                {
                    Instantiate(keyRooms[Random.Range(0, keyRooms.Length)], posTopLeft, Quaternion.identity);
                    keyRoom = true;
                }
                else if (rand == 3)
                {
                    Instantiate(doorRooms[Random.Range(0, doorRooms.Length)], posTopLeft, Quaternion.identity);
                    doorRoom = true;
                }
                else if (rand == 4)
                {
                    Instantiate(pickupRooms[Random.Range(0, pickupRooms.Length)], posTopLeft, Quaternion.identity);
                    pickupRoom = true;
                }
                else //room already chosen
                {
                    rand = Random.Range(1, 2);
                    if(startRoom && keyRoom)
                    {
                        if (rand == 1)
                        {
                            Instantiate(doorRooms[Random.Range(0, doorRooms.Length)], posTopLeft, Quaternion.identity);
                            doorRoom = true;
                        }
                        else if (rand == 2)
                        {
                            Instantiate(pickupRooms[Random.Range(0, pickupRooms.Length)], posTopLeft, Quaternion.identity);
                            pickupRoom = true;
                        }
                    }
                    else if (startRoom && doorRoom)
                    {
                        if (rand == 1)
                        {
                            Instantiate(keyRooms[Random.Range(0, keyRooms.Length)], posTopLeft, Quaternion.identity);
                            keyRoom = true;
                        }
                        else if (rand == 2)
                        {
                            Instantiate(pickupRooms[Random.Range(0, pickupRooms.Length)], posTopLeft, Quaternion.identity);
                            pickupRoom = true;
                        }
                    }
                    else if (startRoom && pickupRoom)
                    {
                        if (rand == 1)
                        {
                            Instantiate(keyRooms[Random.Range(0, keyRooms.Length)], posTopLeft, Quaternion.identity);
                            keyRoom = true;
                        }
                        else if (rand == 2)
                        {
                            Instantiate(doorRooms[Random.Range(0, doorRooms.Length)], posTopLeft, Quaternion.identity);
                            doorRoom = true;
                        }
                    }
                    else if (keyRoom && doorRoom)
                    {
                        if (rand == 1)
                        {
                            Instantiate(startRooms[Random.Range(0, startRooms.Length)], posTopLeft, Quaternion.identity);
                            startRoom = true;
                        }
                        else if (rand == 2)
                        {
                            Instantiate(pickupRooms[Random.Range(0, pickupRooms.Length)], posTopLeft, Quaternion.identity);
                            pickupRoom = true;
                        }
                    }
                    else if (keyRoom && pickupRoom)
                    {
                        if (rand == 1)
                        {
                            Instantiate(keyRooms[Random.Range(0, keyRooms.Length)], posTopLeft, Quaternion.identity);
                            keyRoom = true;
                        }
                        else if (rand == 2)
                        {
                            Instantiate(doorRooms[Random.Range(0, doorRooms.Length)], posTopLeft, Quaternion.identity);
                            doorRoom = true;
                        }
                    }
                    else if (doorRoom && pickupRoom)
                    {
                        if (rand == 1)
                        {
                            Instantiate(keyRooms[Random.Range(0, keyRooms.Length)], posTopLeft, Quaternion.identity);
                            keyRoom = true;
                        }
                        else if (rand == 2)
                        {
                            Instantiate(startRooms[Random.Range(0, startRooms.Length)], posTopLeft, Quaternion.identity);
                            startRoom = true;
                        }
                    }
                }
            }

            else if (i == 3) //top right selection
            {
                if (!startRoom)
                {
                    Instantiate(startRooms[Random.Range(0, startRooms.Length)], posTopRight, Quaternion.identity);
                    startRoom = true;
                }
                else if (!keyRoom)
                {
                    Instantiate(keyRooms[Random.Range(0, keyRooms.Length)], posTopRight, Quaternion.identity);
                    keyRoom = true;
                }
                else if (!doorRoom)
                {
                    Instantiate(doorRooms[Random.Range(0, doorRooms.Length)], posTopRight, Quaternion.identity);
                    doorRoom = true;
                }
                else if (!pickupRoom)
                {
                    Instantiate(pickupRooms[Random.Range(0, pickupRooms.Length)], posTopRight, Quaternion.identity);
                    pickupRoom = true;
                }
            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterData : MonoBehaviour
{
    public int CharacterID = 1001;
    public Character target;

    void Start()
    {
        if(CharacterID < 2000)
        {
            GetComponent<Player>().Initialize(CharacterID);
        }
        else
        {
            GetComponent<Enemy>().Initialize(CharacterID);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontDestroyOnLoadScript : MonoBehaviour
{
    [SerializeField]
    private GameObject instance;

    void Start()
    {
        if(instance != null)
        {
            Destroy(instance);
        }
        else
        {
        DontDestroyOnLoad(instance);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }


}

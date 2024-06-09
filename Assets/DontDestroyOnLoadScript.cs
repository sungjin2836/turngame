using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontDestroyOnLoadScript : MonoBehaviour
{
    [SerializeField]
    private GameObject instance;

    void Start()
    {
        DontDestroyOnLoad(instance);
    }

    // Update is called once per frame
    void Update()
    {
        
    }


}

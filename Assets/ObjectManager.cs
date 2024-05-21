using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectManager : MonoBehaviour
{
    [SerializeField]
    private GameObject _Object;
    private bool isDestroy;
    void Start()
    {
        isDestroy = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void checkObject()
    {
        if (!_Object.active)
        {
            StartCoroutine(GenObject());
            isDestroy = true;
        }
    }


    IEnumerator GenObject()
    {
        while (isDestroy)
        {
            yield return new WaitForSeconds(5.0f);
            ObjectTrue();
        }
    }

    private void ObjectTrue()
    {
        Debug.Log(isDestroy);
        _Object.SetActive(true);
        isDestroy = false;
    }

}

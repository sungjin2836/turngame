using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerMove : MonoBehaviour
{
    float Speed = 5f;
    Rigidbody rbody;
    Rotate rotate;
    [SerializeField]
    private GameObject targetRange;

    void Start()
    {
        rbody = GetComponent<Rigidbody>();
        //rotate = GetComponent<Rotate>();
        targetRange.SetActive(false);

    }


    void Update()
    {
        float xMove = Input.GetAxis("Horizontal");
        float zMove = Input.GetAxis("Vertical");

        
        Vector3 vector3 = new Vector3(xMove, 0, zMove) * Speed;

        this.rbody.velocity = vector3;

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            targetRange.SetActive(true);
            Invoke("TargetActive", 1f);
        }
    }


    private void TargetActive()
    {
        targetRange.SetActive(false);
    }



    
}

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

    public float mouseSensitivity = 100f;
    private float xRotation = 0f;
    private float yRotation = 0f;

    void Start()
    {
        rbody = GetComponent<Rigidbody>();

        UnityEngine.Cursor.lockState = CursorLockMode.Locked;

        targetRange.SetActive(false);

    }


    void Update()
    {
        Movement();
    }


    private void Movement()
    {
        // �Ͻ������� �� ������ ��� ��Ȱ��ȭ
        if (Pause.isPause)
        {
            UnityEngine.Cursor.lockState = CursorLockMode.None;
            return;
        }

        UnityEngine.Cursor.lockState = CursorLockMode.Locked;

        float xMove = Input.GetAxis("Horizontal");
        float zMove = Input.GetAxis("Vertical");

        Vector3 moveDirection = new Vector3(xMove, 0, zMove).normalized;
        Vector3 moveVelocity = transform.TransformDirection(moveDirection) * Speed;

        //Vector3 vector3 = new Vector3(xMove, 0, zMove).normalized * Speed;

        rbody.velocity = new Vector3(moveVelocity.x, rbody.velocity.y, moveVelocity.z);

        //���콺 ȸ��
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f); // ȸ���� ����

        yRotation += mouseX;

        transform.localRotation = Quaternion.Euler(0f, yRotation, 0f);
        Camera.main.transform.localRotation = Quaternion.Euler(xRotation, yRotation, 0f); // ī�޶� ȸ���� ���Ʒ� ����

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

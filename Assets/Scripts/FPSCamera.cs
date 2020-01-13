using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPSCamera : MonoBehaviour
{
    [SerializeField] private GameObject _player = default;
    [SerializeField] private float _sensitivity = 2f;
    private Vector2 _mouseLook;

    // Update is called once per frame
    void Update()
    {
        if (!_player.GetComponent<Player>().canMove)
            return;

        float horizontal = Input.GetAxis("Mouse X");
        float vertical = Input.GetAxis("Mouse Y");
        Vector2 look = new Vector2(horizontal, vertical);
        _mouseLook += look * _sensitivity;
        _mouseLook.y = Mathf.Clamp(_mouseLook.y, -80f, 80f);
        transform.localRotation = Quaternion.AngleAxis(-_mouseLook.y, Vector3.right);
        _player.transform.localRotation = Quaternion.AngleAxis(_mouseLook.x, _player.transform.up);
    }
}

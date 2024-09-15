using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem; 

public class CameraDrag : MonoBehaviour
{
    private Vector3 _origin; 
    private Vector3 _difference;

    private Camera _camera;

    private bool _isDragging; 

    private void Awake(){
        _camera = Camera.main; 
    } 

    public void OnDrag(InputAction.CallbackContext ctx){
        if(ctx.started){
            _origin = GetMousePosition; 
        }
        _isDragging = ctx.started || ctx.performed; 
    }

    private void LateUpdate(){
        if (!_isDragging) return; 
        _difference = GetMousePosition - transform.position; 
        transform.position = _origin - _difference; 
    }

    private Vector3 GetMousePosition => _camera.ScreenToWorldPoint((Vector3)Mouse.current.position.ReadValue()); 
}

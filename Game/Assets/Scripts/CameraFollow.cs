using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] float speedFollow = 3f;
    [SerializeField] Transform target;
    [SerializeField] float minY,maxY,maxX,minX;
    Animator anim;
    public static CameraFollow instance;

    private void Awake() {
        instance = this;
    }

    private void Start() {
        anim = GetComponent<Animator>();
    }
    private void Update() {
        transform.position = Vector3.Lerp(transform.position,
        new Vector3(
            Mathf.Clamp(target.position.x,minX,maxX),
            Mathf.Clamp(target.position.y,minY,maxY),
            -10),
            speedFollow * Time.deltaTime);
             
    }
    public void cameraShake()
    {
        anim.Play("CameraShake");
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonCamera : MonoBehaviour
{
    Transform target;
    public Vector3 rotationToTarget;

    [SerializeField] Vector3 offset = new Vector3(0, 8, -8f);
    [SerializeField] Vector3 rotationOffset = new Vector3(0, 8, -8f);

    void Start()
    {
        target = _SceneManager.singleton.player.transform;
    }

    private void Update()
    {
        rotationToTarget += new Vector3(0, _SceneManager.singleton.lookDelta.x);
    }


    void LateUpdate()
    {
        transform.position = target.transform.position + Quaternion.Euler(rotationToTarget) * offset;
        Quaternion baseRotation = Quaternion.LookRotation(target.position - transform.position, Vector3.up);
        transform.rotation = Quaternion.Euler(baseRotation.eulerAngles + rotationOffset);
    }
}

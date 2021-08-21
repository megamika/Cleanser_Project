using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TopDownCamera : MonoBehaviour
{
    Transform target;
    [SerializeField] Vector3 offset;
    [Range(0.5f,1f)]
    [SerializeField] float scale = 1f;

    private void Start()
    {
        target = _SceneManager.singleton.player.transform;
    }

    private void LateUpdate()
    {
        transform.position = target.transform.position + offset * scale;
    }
}

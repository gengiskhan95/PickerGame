using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    #region Level States

    [Header("Level States")]

    public bool isLevelStart;
    public bool isLevelDone;
    public bool isLevelFail;

    #endregion

    Transform player;
    Transform target;

    #region Camera Settings

    [Header("Camera Settings")]

    [SerializeField] Vector3 offset;
    [SerializeField] float smoothTime;
    [SerializeField] float offSetChangingSpeed = 5;
    [SerializeField] float targetZoomOffSetY = 2;

    #endregion

    private Vector3 velocity = Vector3.zero;
    Vector3 targetPosition;

    public static CameraController instance;

    private void Awake()
    {
        if (!instance)
        {
            instance = this;
        }
    }

    void Start()
    {
        player = PlayerController.instance.transform;
        target = player;
    }

    void LateUpdate()
    {
        if (isLevelStart && !isLevelDone && !isLevelFail)
        {
            targetPosition.x = target.transform.position.x + offset.x;
            targetPosition.y = target.transform.position.y + offset.y;

            transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);
        }
    }
}
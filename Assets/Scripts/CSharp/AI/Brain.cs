using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using Ludiq;

[IncludeInSettings(true)]
public class Brain : MonoBehaviour
{
    Seeker seeker;
    Entity entity;
    
    Task currentTask;

    private void Awake()
    {
        seeker = GetComponent<Seeker>();
        entity = GetComponent<Entity>();
    }

    void Start()
    {

    }

    private void Update()
    {
        switch (currentTask)
        {
            case Task.None:
                break;
            case Task.FollowTransform:
                FollowTransformUpdate();
                break;
            default:
                break;
        }
    }

    #region tasks


    #region Follow transform

    Transform transformToFollow;
    float followDistance;

    public void FollowTransform(Transform transform, float atDistance)
    {
        currentTask = Task.FollowTransform;
        transformToFollow = transform;
        followDistance = atDistance;
        StartPath(transform.position, transformToFollow.position);
    }

    Vector2 smoothDeltaPosition;

    void FollowTransformUpdate()
    {
        if (Vector3.Distance(transformToFollow.position, currentEnd) > updateDestinationLength)
        {
            StartPath(transform.position, transformToFollow.position);
        }

        FollowCurrentPath();

        if (Vector3.Distance(transform.position, transformToFollow.position) < followDistance || reachedEndOfPath)
        {
            entity.MoveTo(0f, 0f);
        }
        else
        {
            entity.MoveTo(pathfindDirection.x, pathfindDirection.z);
        }
    }

    void FollowTransformCancel()
    {
        entity.MoveTo(0, 0);
    }

    #endregion

    enum Task
    {
        None,
        FollowTransform
    }

    public void StopTask()
    {
        switch (currentTask)
        {
            case Task.None:
                break;
            case Task.FollowTransform:
                FollowTransformCancel();
                break;
            default:
                break;
        }
        currentTask = Task.None;
    }

    #endregion

    #region general pathfinding


    public float updateDestinationLength = 0.05f;
    public float reachedWaypointDistance = 3f;

    int currentWaypoint = 0;
    Path currentPath;

    Vector3 currentStart;
    Vector3 currentEnd;

    Vector3 pathfindDirection;

    public void OnPathComplete(Path p)
    {
        if (!p.error)
        {
            currentPath = p;
            currentWaypoint = 0;
        }
    }

    void StartPath(Vector3 start, Vector3 end) 
    {
        currentStart = start;
        currentEnd = end;
        seeker.StartPath(start, end, OnPathComplete); 
    }

    bool reachedEndOfPath;

    void FollowCurrentPath()
    {
        if (currentPath == null)
            return;

        reachedEndOfPath = false;

        float distanceToWaypoint;
        while (true)
        {
            distanceToWaypoint = Vector3.Distance(transform.position, currentPath.vectorPath[currentWaypoint]);
            if (distanceToWaypoint < reachedWaypointDistance)
            {
                if (currentWaypoint + 1 < currentPath.vectorPath.Count)
                {
                    currentWaypoint++;
                }
                else
                {
                    reachedEndOfPath = true;
                    break;
                }
            }
            else
            {
                break;
            }
        }

        pathfindDirection = (currentPath.vectorPath[currentWaypoint] - new Vector3(transform.position.x, currentPath.vectorPath[currentWaypoint].y, transform.position.z)).normalized;
    }

    #endregion
}

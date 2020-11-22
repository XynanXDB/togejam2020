using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Game.Utility;
using Game.Library.Delegate;

namespace Game.Core.Movement
{
    public enum EPathingCondition {Completed, Stuck}

[RequireComponent(typeof(NavMeshAgent))]
public class NavAgentHandler : MonoBehaviour
{
    [SerializeField] protected float NavRunBurstModifier = 3.0f;
    [SerializeField] protected Rigidbody TargetRigidbody;

    [Header("Pathing")]
    [SerializeField] protected float PathingPollTime = 0.5f;
    [SerializeField, Range(0, 10)] protected int PathingStuckTimeoutTickThreshold = 5;

    private int _IsStuck;
    public bool IsStuck { get { return _IsStuck > 0; }}

    private NavMeshAgent NavAgent;
    private Vector3 PreviousLocation;
    private Coroutine CheckStuckCoroutine;
    private UController OwningController;
    public OneParamSignature<EPathingCondition> OnPathingEnd;

    void Reset()
    {
        if (!TryGetComponent<NavMeshAgent>(out NavAgent))
            NavAgent = gameObject.AddComponent<NavMeshAgent>();
    }

    void Awake()
    {
        TryGetComponent<NavMeshAgent>(out NavAgent);

        OnPathingEnd += HandleOnPathEnd;
    }

    void OnDestroy()
    {
        StopAllCoroutines();
    }

    public void SetDestination(Vector3 Location)
    {
        NavAgent.SetDestination(Location);
        PreviousLocation = Vector3.positiveInfinity;
        _IsStuck = 0;
        
        if (CheckStuckCoroutine == null)
            CheckStuckCoroutine = StartCoroutine("CheckStuck");
    }

    protected virtual void OnRun(int Value)
    {
        if (Value == 0)
        {
            NavAgent.acceleration = 20.0f;
            NavAgent.speed = 10.0f;
            NavAgent.angularSpeed = 360.0f; 
        }
        else
        {
            NavAgent.acceleration = 60.0f;
            NavAgent.speed = 30.0f;
            NavAgent.angularSpeed = 1080.0f;
        }
    }

    void HandleOnPathEnd(EPathingCondition Condition)
    {
        if (Condition == EPathingCondition.Stuck)
            NavAgent.ResetPath();
        
        StopCoroutine(CheckStuckCoroutine);
        CheckStuckCoroutine = null;

        Debug.Log(Condition);
    }

    IEnumerator CheckStuck()
    {
        while (true)
        {   
            Vector3 CurrentLocation = gameObject.transform.position;

            if (!IsStuck && !NavAgent.hasPath && !NavAgent.pathPending && NavAgent.remainingDistance < 0.1f)
            {
                OnPathingEnd?.Invoke(EPathingCondition.Completed);
                
                _IsStuck = 0;
                yield break;
            }

            if (!MathHelpers.Vector3Equals(PreviousLocation, CurrentLocation, 0.1f))
            {
                PreviousLocation = CurrentLocation;
                _IsStuck = 0;
            }
            else
            {
                if (_IsStuck > PathingStuckTimeoutTickThreshold)
                {
                    OnPathingEnd?.Invoke(EPathingCondition.Stuck);

                    yield break;
                }
                else
                    _IsStuck ++;
            }

            yield return new WaitForSeconds(PathingPollTime);
        }
    }

    public bool IsMoving { get { return NavAgent.velocity.magnitude > 0; }}
}
}

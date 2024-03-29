using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using FiniteStateMachine;

public class EnemyPathfinding : MonoBehaviour
{
    //reference to the NavMeshAgent attached to this object
    private NavMeshAgent agent;

    //object the AI agent is trying to navigate towards
    [SerializeField] GameObject navPoint, player;

    //floats relating to speed of objects

    //floats relating to detction distances
    [SerializeField] float stoppingDistance,
        detectionDistance, 
        runSpeed,
        walkSpeed,
        float5,
        hootenani;

    public StateMachine StateMachine { get; private set; }

    private Animator animC;

    private void Awake()
    {
        StateMachine = new StateMachine();

        if (!TryGetComponent<NavMeshAgent>(out agent))
        {
            Debug.LogError("This object needs a navmesh agent attached to it");
            
        }

        animC = GetComponentInChildren<Animator>();
        

    }

    // Start is called before the first frame update
    void Start()
    {
        StateMachine.SetState(new IdleState(this));
        agent.isStopped = true;

    }

    // Update is called once per frame
    void Update()
    {
        StateMachine.OnUpdate();
    }

    private Vector3 UpdateTargetPosition(GameObject targetPos)
    {
        return targetPos.transform.position;
    }

    public abstract class EnemyMoveState : IState
    {
        protected EnemyPathfinding instance;

        public EnemyMoveState(EnemyPathfinding _instance)
        {
            instance = _instance;
        }

        public virtual void OnEnter()
        {
            
        }

        public virtual void OnExit()
        {
            
        }

        public virtual void OnUpdate()
        {
            
        }
    }

    public class MoveState : EnemyMoveState
    {
        public MoveState(EnemyPathfinding _instance) : base(_instance)
        {

        }

        public override void OnEnter()
        {
            //set the agent to 'stopped'
            instance.agent.isStopped = false;
            Debug.Log("Entering MoveState");

            instance.agent.speed = instance.walkSpeed;

            instance.animC.SetTrigger("Walk");
        }

        public override void OnUpdate()
        {
            //update the position of the target object
            //move towards it
            if (
                Vector3.Distance
                (instance.transform.position, 
                instance.player.
                transform.position) < instance.detectionDistance)
            {
                instance.StateMachine.SetState(new ChaseState(instance));
            }
            else if (Vector3.Distance(instance.transform.position, instance.navPoint.transform.position) > instance.stoppingDistance)
            {
                instance.agent.SetDestination(instance.navPoint.transform.position);
            }
            else
            {
                //set the state to IdleState
                instance.StateMachine.SetState(new IdleState(instance));
            }
        }

        public override void OnExit()
        {
            instance.animC.ResetTrigger("Walk");
        }
    }

    public class IdleState : EnemyMoveState
    {
        public IdleState(EnemyPathfinding _instance) : base(_instance)
        {
        }

        public override void OnEnter()
        {
            instance.agent.isStopped = true;
            Debug.Log("Entering IdleState");

            instance.animC.SetTrigger("Idle");
        }

        public override void OnUpdate()
        {
            if(Vector3.Distance(instance.transform.position, instance.player.transform.position) < instance.detectionDistance)
            {
                instance.StateMachine.SetState(new ChaseState(instance));
            }            
            else if(Vector3.Distance(instance.transform.position, instance.navPoint.transform.position) > instance.stoppingDistance)
            {
                //switch to the MoveState
                instance.StateMachine.SetState(new MoveState(instance));
            }
        }

        public override void OnExit()
        {
            instance.animC.ResetTrigger("Idle");
        }
    }

    public class ChaseState : EnemyMoveState
    {
        public ChaseState(EnemyPathfinding _instance) : base(_instance)
        {
        }

        public override void OnEnter()
        {
            Debug.Log("Entering ChaseState");
            instance.agent.isStopped = false;

            instance.agent.speed = instance.runSpeed;

            instance.animC.SetTrigger("Run");
        }

        public override void OnUpdate()
        {
            if(Vector3.Distance(instance.transform.position, instance.player.transform.position) < instance.detectionDistance)
            {
                instance.agent.SetDestination(instance.player.transform.position);
            }
            else
            {
                instance.StateMachine.SetState(new IdleState(instance));
            }
        }

        public override void OnExit()
        {
            instance.animC.ResetTrigger("Run");
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionDistance);
    }

}

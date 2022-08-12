using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

using Baponkar.BehaviorTree;


public class AiBehavior : MonoBehaviour
{
    #region Variables

    public Transform player;
    public float seeRange = 10f;

    NavMeshAgent agent;
    
    public Transform [] waypoints;
    private Transform currentWaypoint;
    private int currentWaypoint_index = 0;

    

    BehaviorTree tree;
    Node.Status treeStatus = Node.Status.Running;

    #endregion

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        currentWaypoint = waypoints[currentWaypoint_index];

        tree = new BehaviorTree();

        SequenceNode patrol =  new SequenceNode("Patrol");

        SelectorNode moveToWaypoint1 = new SelectorNode("Move to Waypoint1");
        SelectorNode moveToWaypoint2 = new SelectorNode("Move to Waypoint2");
        SelectorNode moveToWaypoint3 = new SelectorNode("Move to Waypoint3");
        SelectorNode moveToWaypoint4 = new SelectorNode("Move to Waypoint4");
        
        LeafNode moveToCurrentWaypoint1 = new LeafNode("Move to 1st Waypoint", MoveToCurrentWaypoint);
        LeafNode chasePlayer1 = new LeafNode("Chase Player", ChasePlayer);
        
        LeafNode moveToCurrentWaypoint2 = new LeafNode("Move to 2nd Waypoint", MoveToCurrentWaypoint);
        LeafNode chasePlayer2 = new LeafNode("Chase Player", ChasePlayer);
        
        LeafNode moveToCurrentWaypoint3 = new LeafNode("Move to 3rd Waypoint", MoveToCurrentWaypoint);
        LeafNode chasePlayer3 = new LeafNode("Chase Player", ChasePlayer);

        LeafNode moveToCurrentWaypoint4 = new LeafNode("Move to 4th Waypoint", MoveToCurrentWaypoint);
        LeafNode chasePlayer4 = new LeafNode("Chase Player", ChasePlayer);
        

        moveToWaypoint1.AddChild(moveToCurrentWaypoint1);
        moveToWaypoint1.AddChild(chasePlayer1);

        moveToWaypoint2.AddChild(moveToCurrentWaypoint2);
        moveToWaypoint2.AddChild(chasePlayer2);

        moveToWaypoint3.AddChild(moveToCurrentWaypoint3);
        moveToWaypoint3.AddChild(chasePlayer3);

        moveToWaypoint4.AddChild(moveToCurrentWaypoint4);
        moveToWaypoint4.AddChild(chasePlayer4);

        patrol.AddChild(moveToWaypoint1);
        patrol.AddChild(moveToWaypoint2);
        patrol.AddChild(moveToWaypoint3);
        patrol.AddChild(moveToWaypoint4);

        tree.AddChild(patrol);



    }

    
    void Update()
    {
        treeStatus = tree.Process();
    }

    void Patrol()
    {
        if(currentWaypoint != null)
        {
            agent.SetDestination(currentWaypoint.position);
        }

        if(agent.remainingDistance <= agent.stoppingDistance)
        {
            if(currentWaypoint_index >= waypoints.Length - 1)
            {
                currentWaypoint_index = 0;
            }
            else
            {
                currentWaypoint_index++;
            }
            currentWaypoint = waypoints[currentWaypoint_index];
        }
    }

    Node.Status ChasePlayer()
    {
        agent.SetDestination(player.position);
        if(agent.remainingDistance <= agent.stoppingDistance)
        {
            treeStatus = Node.Status.Success;
        }
        return Node.Status.Running;
    }

    float GetDistanceToPlayer()
    {
        return Vector3.Distance(player.position, transform.position);
    }

    Node.Status PlayerSeen()
    {
        float playerDistance = GetDistanceToPlayer();
        if(playerDistance <= seeRange)
        {
            return Node.Status.Success;
        }
        else
        {
            return Node.Status.Failure;
        }
    }

    Node.Status MoveToCurrentWaypoint()
    {
        if(currentWaypoint != null)
        {
            currentWaypoint = waypoints[currentWaypoint_index];
            Debug.Log("Moving towards : " + currentWaypoint.name);
            agent.SetDestination(currentWaypoint.position);

            if(agent.remainingDistance <= agent.stoppingDistance)
            {
                if(currentWaypoint_index >= waypoints.Length - 1)
                {
                    currentWaypoint_index = 0;
                }
                else
                {
                    currentWaypoint_index++;
                }
                return Node.Status.Success;
            }
        }
        
        float playerDistance = GetDistanceToPlayer();

        if(playerDistance <= seeRange)
        {
            return Node.Status.Failure;
        }

        return Node.Status.Running;
    }
}

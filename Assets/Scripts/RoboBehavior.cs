/*
This Robo Behavior is used to control the robot.
 * Donloaded from : https://github.com/baponkar/My-Behavior-Tree
 * Here robot steal the diamond if its money is less than 500 and 
 * if he successfully steal the diamond i.e take diamond to the van,
 * then he will earn 500.
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

using Baponkar.BehaviorTree;

public class RoboBehavior : MonoBehaviour
{
    BehaviorTree tree;
    NavMeshAgent agent;

    [Range(0, 1000f)]
    public float money = 800f;
    public GameObject diamond;
    public GameObject backDoor;
    public GameObject fontDoor;
    public GameObject van;

    public enum ActionState { Idle, WORKING };
    public ActionState actionState = ActionState.Idle;

    Node.Status treeStatus = Node.Status.Running;
    
    void Start()
    {
    /*
    Used Flowchart to create the behavior tree.
                        tree(Root Node)
                            |
                     steal(Sequence Node)
                            |
                    ----------------------------------------------------------------------------
                    |                        |                            |                    |
        (Decision Leaf)?hasGotMoney     goToDoor(Selector Node)  goToDiamond(Leaf Node)   goToVan(Leaf Node)  
                                    |
                                -----------------
                                |               |
                    goToBackDoor(Leaf Node)    goToFontDoor(Leaf Node)               

    */ 
       agent = GetComponent<NavMeshAgent>(); 
       tree = new BehaviorTree();

       SequenceNode stel = new SequenceNode("stel something");
       LeafNode hasGotMoney = new LeafNode("has got money", HasMoney);
       LeafNode goToBackDoor = new LeafNode("go to back door", GoToBackDoor);
       LeafNode goToFontDoor = new LeafNode("go to font door", GoToFontDoor);
       LeafNode goToDiamond = new LeafNode("go to diamond", GoToDiamond);
       LeafNode goToVan = new LeafNode("go to van", GoToVan);

       SelectorNode openDoor = new SelectorNode("open door");
       openDoor.AddChild(goToBackDoor);
       openDoor.AddChild(goToFontDoor);
       
       stel.AddChild(hasGotMoney);
       stel.AddChild(openDoor);
       stel.AddChild(goToDiamond);
       stel.AddChild(goToVan);
       
       tree.AddChild(stel);

       tree.PrintTree();
    }

    
    void Update()
    {
        Debug.Log("Tree status : " + treeStatus);
        if(treeStatus != Node.Status.Success)
        {
            treeStatus = tree.Process();
        }
    }

    Node.Status GoToDestination(Vector3 destination)
    {
       float distanceToTarget = agent.remainingDistance;
       //float distanceToTarget = Vector3.Distance(destination, this.transform.position);

       if(actionState == ActionState.Idle)
       {
           agent.SetDestination(destination);
           actionState = ActionState.WORKING;
       }
       else if(Vector3.Distance(this.transform.position, destination) >= 2f && agent.speed < 0.1f)
       {
           actionState = ActionState.Idle;
           return Node.Status.Failure;  
       }
       else if(distanceToTarget <  2f)
       {
           actionState = ActionState.Idle;
           return Node.Status.Success;
       }

       
       return Node.Status.Running;
    }

    Node.Status GoToBackDoor()
    {
        return GoToDoor(backDoor);
    }
    
    Node.Status GoToFontDoor()
    {
        return GoToDoor(fontDoor);
    }

    Node.Status HasMoney()
    {
        if(money >= 500f)
        {
            return Node.Status.Failure;
        }
        return Node.Status.Success;
    }


    Node.Status GoToDoor(GameObject door)
    {
        Node.Status s =  GoToDestination(door.transform.position);
        if(s == Node.Status.Success) //if reached destination
        {
            if(!door.GetComponent<Lock>().isLocked) //if door is unlocked
            {
                door.SetActive(false);
                return Node.Status.Success;
            }
            else //if door is locked
            {
                return Node.Status.Failure;
            }
        }
        else
        {
            return s; //returning either running or failure
        }
    }

    Node.Status GoToDiamond()
    {
        Node.Status s = GoToDestination(diamond.transform.position);
        if(s == Node.Status.Success)
        {
            diamond.transform.SetParent(this.transform, false);
            diamond.transform.localPosition = Vector3.zero;
            diamond.transform.localRotation = Quaternion.identity;
        }
        return s;
    }
   

    public Node.Status GoToVan()
    {
        Node.Status s = GoToDestination(van.transform.position);
        if(s == Node.Status.Success)
        {
            Destroy(diamond);
            money += 500f;
        }
        return s;
    }
}

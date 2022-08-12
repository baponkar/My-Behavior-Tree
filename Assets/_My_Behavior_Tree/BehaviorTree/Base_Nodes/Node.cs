/*
* a Node is a tree node.
* It has name, children, current child, and process method.
* It has Process method which is used to process the node.
* It has Tick method which is used to process the node.
* It has Status enum which is used to represent the status of the node.
* It has Status.Success, Status.Failure, Status.Running.
* Donloaded from : https://github.com/baponkar/My-Behavior-Tree
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Baponkar.BehaviorTree
{
    public class Node
    {
        public enum Status { Success, Failure, Running };
        public Status status;

        public List<Node> children = new List<Node>();

        public int currentChild = 0;
        public string name;

        public Node() 
        {

        }
        public Node(string name)
        {
            this.name = name;
        }

        public virtual Status Start()
        {
            return Status.Running;
        }

        public virtual Status Update()
        {
            return Status.Running;
        }   

        

        public virtual Status Process()
        {
            return children[currentChild].Process();
        }

        public void AddChild(Node child)
        {
            children.Add(child);
        }
    }
}

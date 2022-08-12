/*
* A Behavior tree is a tree data structure which is used to implement behavior tree.
* It has root node and current node.
* It has Tick method which is used to process the tree.
* It has Status enum which is used to represent the status of the tree.
* It has Status.Success, Status.Failure, Status.Running.
 * Donloaded from : https://github.com/baponkar/My-Behavior-Tree
*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Baponkar.BehaviorTree
{
    public class BehaviorTree : Node
    {
        public BehaviorTree()
        {
            name = "Tree";
        }

        public BehaviorTree(string name)
        {
            this.name = name;
        }

        public override Status Process()
        {
            return children[currentChild].Process();
        }

        struct NodeLevel
        {
            public int level;
            public Node node;
        }

        public void PrintTree()
        {
            string treePrintOut = "";
            Stack<NodeLevel> nodeStack = new Stack<NodeLevel>();
            Node currentNode = this;

            nodeStack.Push(new NodeLevel { level = 0, node = currentNode });

            while(nodeStack.Count > 0)
            {
                NodeLevel nextNode = nodeStack.Pop();
                treePrintOut += new string('-',nextNode.level) + nextNode.node.name + "\n";

                for (int i = nextNode.node.children.Count - 1; i >= 0; i--)
                {
                    nodeStack.Push(new NodeLevel {level = nextNode.level + 1 ,node = nextNode.node.children[i]});
                }
            }
            Debug.Log(treePrintOut);
        }
    }
}

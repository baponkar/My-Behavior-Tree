
/* Sequence node is like and gate if every child node is success, then the sequence node is success.
 * If one child node is failure, then the sequence node is failure.
 * Donloaded from : https://github.com/baponkar/My-Behavior-Tree
 */


using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Baponkar.BehaviorTree
{
    public class SequenceNode : Node
    {
        public SequenceNode(string name)
        {
            this.name = name;
        }
        
        public override Status Process()
        {
            Status childStatus = children[currentChild].Process();
            if(childStatus == Status.Running)
            {
                return Status.Running;
            }
            
            if(childStatus == Status.Failure)
            {
                return childStatus;
            }

            currentChild++;
            if(currentChild >= children.Count)
            {
                currentChild = 0;
                return Status.Success;
            }
            return Status.Running;
        }
    }
}

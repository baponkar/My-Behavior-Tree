/* Selector node is like OR gate if one child node is success, then the selector node is success.
 * If all child node is failure, then the selector node is failure.
  * Donloaded from : https://github.com/baponkar/My-Behavior-Tree
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Baponkar.BehaviorTree
    {
    public class SelectorNode : Node
    {
        public SelectorNode(string name)
        {
            this.name = name;
        }
        public override Status Process()
        {
            Status childStatus = children[currentChild].Process();

            if (childStatus == Status.Running)
            {
                return Status.Running;
            }

            if(childStatus == Status.Success)
            {
                currentChild = 0;
                return Status.Success;
            }

            if (childStatus == Status.Failure)
            {
                currentChild++;
                if (currentChild >= children.Count)
                {
                    currentChild = 0;
                    return Status.Failure;
                }
                return Status.Running;
            }
            return Status.Success;
        }
    }
}

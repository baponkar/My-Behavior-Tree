/* Leaf Node is lower level node, it has no children.
 * Leaf node is used to implement behavior tree.
  * Donloaded from : https://github.com/baponkar/My-Behavior-Tree
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Baponkar.BehaviorTree
{
    public class LeafNode : Node
    {
        public delegate Status Tick(); //delegate is container of methods
        public Tick processMethod;

        public LeafNode() { }

        public LeafNode(string name, Tick processMethod)
        {
            this.name = name;
            this.processMethod = processMethod;
        }
    

        public override Status Process()
        {
            if(processMethod != null)
            {
                return processMethod();
            }
            return Status.Failure;
        }
    }
}

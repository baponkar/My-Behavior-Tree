using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Baponkar.BehaviorTree
{
    public class InverterNode : Node
    {
        public InverterNode(string name)
        {
            this.name = name;
        }

        public override Status Process()
        {
            Status childStatus = children[0].Process(); //As inverter has only single child node
            switch (childStatus)
            {
                case Node.Status.Running:
                    return Status.Running;
                case Node.Status.Success:
                    return Status.Failure;
                case Node.Status.Failure:
                    return Status.Success;
                default:
                    break;
            }
            return Status.Failure;
        }
    }
}

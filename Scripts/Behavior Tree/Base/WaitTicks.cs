using Godot;
using System;


namespace BehaviorTree.Base
{
    public class WaitTicks : Node, IBehaviorNode
    {
        [Export]
        private readonly int ticksToWait;

        private int current = 0;

        public States NodeState { get; set; }

        public void InitNode(in TreeController controller)
        {
            
        }

        public States Tick(in TreeController controller)
        {
            if(ticksToWait > current){
                current++;
                controller.ExitNode(this, States.RUNNING);
                
            }else{
                controller.ExitNode(this, States.SUCCESS);
                MyConsole.Write("Leaving waittick");
                this.current = 0;
            }

            return this.NodeState;
        }
    }
}


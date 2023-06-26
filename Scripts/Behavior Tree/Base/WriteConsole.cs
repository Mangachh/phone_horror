using Godot;
using System;


namespace BehaviorTree.Base
{
    public class WriteConsole : Node, IBehaviorNode
    {
        public States NodeState { get; set; }

        [Export]
        private readonly string text;

        public void InitNode(in TreeController controller)
        {
            
        }

        public States Tick(in TreeController controller)
        {
            MyConsole.Write(this.text);
            controller.ExitNode(this, States.SUCCESS);
            return this.NodeState;
        }
    }
}


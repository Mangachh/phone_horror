using Godot;
using System;
using BehaviorTree.Base;
using Apps.Messages;
using MySystems;

namespace BehaviorTree.AppMessages
{
    // esto tendría que ser un sequence
    public class Choice : SequenceNode, IBehaviorNode
    {
        [Export]
        public string Text { get; private set; }

        private bool sended = false;

        public override States Tick(in TreeController controller)
        {
            ChatsController mess = SystemManager.GetInstance(this).GetSystem<ChatsController>();
            //mess.SendMessage(this.text, this.isRight);
            if (sended == false)
            {
                mess.SendMessage(this.Text, true);
                sended = true;
            }



            this.NodeState = base.Tick(controller);

            if(this.NodeState != States.RUNNING){
                sended = false;
            }

            controller.ExitNode(this, this.NodeState);
            return NodeState;
        }
    }

}

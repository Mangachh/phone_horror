using Godot;
using System;
using BehaviorTree.Base;
using MySystems;
using Apps.Messages;


namespace BehaviorTree.AppMessages
{
    public class WriteMessage : Node, IBehaviorNode
    {
        public States NodeState { get; set; }

        [Export]
        public string Text{get; private set;}

        [Export]
        private bool isRight;

        [Export]
        private float time;

        bool written = false;


        // Called when the node enters the scene tree for the first time.
        public override void _Ready()
        {

        }

        public void InitNode(in TreeController controller)
        {
            //throw new NotImplementedException();
        }

        public States Tick(in TreeController controller)
        {
            // tendríamos que cambiarlo, pero bueno
            if (this.NodeState == States.RUNNING)
            {
                if (this.written)
                {
                    MyConsole.Write("Time has passed");
                    controller.ExitNode(this, States.SUCCESS);
                    ChatsController mess = SystemManager.GetInstance(this).GetSystem<ChatsController>();
                    mess.UnsubscriteToTime(this.Written);
                    this.written = false;
                }
            }
            else
            {
                ChatsController mess = SystemManager.GetInstance(this).GetSystem<ChatsController>();
                //mess.SendMessage(this.text, this.isRight);
                mess.SendMessageWaitingTime(this.Text, this.isRight, this.time);
                if(this.time == 0){
                    controller.ExitNode(this, States.SUCCESS);
                }else{
                    mess.SubscribeToTime(this.Written);
                    controller.ExitNode(this, States.RUNNING);
                }
                
            }


            return this.NodeState;
        }

        private void Written(int a)
        {
            this.written = true;
        }

    }

}

using Godot;
using System;
using BehaviorTree.Base;
using Apps.Messages;
using MySystems;

namespace BehaviorTree.AppMessages
{
    public class ChoiceSelector : Node, IBehaviorNode
    {
        public States NodeState { get; set; }

        private string[] choices;

        private bool sended = false;
        private int index = -1;


        public void InitNode(in TreeController controller)
        {
            choices = new string[base.GetChildCount()];
            for (int i = 0; i < choices.Length; i++)
            {
                Choice mess = base.GetChild<Choice>(i);
                this.choices[i] = mess.Text;
                mess.InitNode(controller);
            }
        }

        public States Tick(in TreeController controller)
        {
            if (index < 0)
            {
                if (sended == false)
                {
                    ChatsController messCont = SystemManager.GetInstance(this).GetSystem<ChatsController>();
                    messCont.SendChoices(this.choices, OnClick);
                    sended = true;
                }
                controller.ExitNode(this, States.RUNNING);
            }
            else
            {
                IBehaviorNode node = base.GetChild<IBehaviorNode>(this.index);
                this.NodeState = node.Tick(controller);

                if (this.NodeState != States.RUNNING)
                {
                    this.index = -1;
                    this.sended = false;
                }

                controller.ExitNode(this, this.NodeState);
            }

            return this.NodeState;
        }

        private void OnClick(Apps.Messages.Choice choice)
        {
            this.index = choice.Index;
        }
    }
}


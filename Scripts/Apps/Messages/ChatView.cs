using Godot;
using System;
using System.Collections;
using System.Collections.Generic;
using MySystems;

namespace Apps.Messages
{

    public class ChatView : Control, IApp
    {
        // export vars
        // TODO: on production, const strings        

        [Export]
        private readonly string containerPath = "messageApp";

        [Export]
        private readonly string messagePath;

        [Export]
        private readonly string choicePath;

        [Export]
        private readonly string timerPath;

        /// <summary>
        /// The controller of this view
        /// </summary>
        private ChatsController controller;

        /// <summary>
        /// The container of the app
        /// TODO: modify
        /// </summary>
        private ChatContainer container;

        /// <summary>
        /// Message Scene preload
        /// </summary>        
        private PackedScene messageScene;


        /// <summary>
        /// Choice scene preload
        /// </summary>
        private PackedScene choiceScene;

        /// <summary>
        /// Last message written in this view
        /// </summary>
        private Message lastMessage;

        /// <summary>
        /// Timer, used on <see cref="WriteMessageWaitingTime(in string, bool, float)"
        /// </summary>
        private Timer timer;

        /// <summary>
        /// Empty array of choices.
        /// The array is used to create the new choices
        /// in <see cref="WriteChoices(string[], Action{Choice})"/>
        /// </summary>
        Choice[] choices;


        private (string text, bool isRight) toWrite;


        private List<Action<Choice>> clickSubcribors;

        private int maxWidth;

        public override void _EnterTree()
        {
            this.Init();
            MyConsole.Write(this.container);
        }

        public override void _Ready()
        {
            base._Ready();
        }

        /// <summary>
        /// Inits the app
        /// </summary>
        public void Init()
        {
            // preload scenes
            this.messageScene = GD.Load<PackedScene>(this.messagePath);
            this.choiceScene = GD.Load<PackedScene>(this.choicePath);

            // get the "res://addons/behaviour_nodes/behaviour.gd"
            this.container = base.GetNode<ChatContainer>(this.containerPath);
            this.timer = base.GetNode<Timer>(this.timerPath);

            // create controllers
            this.controller = new ChatsController(this);
            timer.Connect("timeout", this, "OnTime");
            this.clickSubcribors = new List<Action<Choice>>();

            SystemManager.GetInstance(this).TryAddSystem(controller);

            this.maxWidth = (int)this.RectSize.x / 2;
        }

        public void Shutdown()
        {
        }

        /// <summary>
        /// Writes a mesage to the app.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="isRight"></param>
        public void WriteMessage(in string message, bool isRight)
        {
            // test Mess
            Message newMessage = this.messageScene.Instance<Message>();

            //this.container.EnterMessageChild(m, this.isRigth);
            this.container.AddChild(newMessage);
            newMessage.WriteMessage(message, this.maxWidth);
            this.container.CallDeferred("PlaceControl", newMessage, isRight, lastMessage);
            this.lastMessage = newMessage;
        }   


        /// <summary>
        /// Sends a message with a waiting time.
        /// While the message is waiting, the app will write something
        /// like "...".
        /// </summary>
        /// <param name="message">Message to write</param>
        /// <param name="isRight">Is it right aligned?</param>
        /// <param name="time">Time to wait</param>
        public void WriteMessageWaitingTime(in string message, bool isRight, float time)
        {
            if (time == 0)
            {
                this.WriteMessage(message, isRight);
            }

            this.WriteMessage("...", isRight);
            timer.WaitTime = time;
            toWrite.text = message;
            toWrite.isRight = isRight;
            timer.Start();

        }

        /// <summary>
        /// Send the choices to the app. 
        /// </summary>
        /// <param name="choicesText">Array of choices text</param>
        /// <param name="OnSelected">Event called when a choice is selected</param>
        public void WriteChoices(string[] choicesText, Action<Choice> OnSelected)
        {
            this.choices = new Choice[choicesText.Length];

            for (int i = 0; i < this.choices.Length; i++)
            {
                this.choices[i] = this.choiceScene.Instance<Choice>();

                this.container.AddChild(this.choices[i]);
                this.choices[i].WriteMessage(choicesText[i], this.maxWidth);
                this.container.CallDeferred("PlaceControl", this.choices[i], true, this.lastMessage);
                this.choices[i].SubscribeOnClick(this.OnClickChoice);
                this.choices[i].Index = i;
                this.lastMessage = this.choices[i];
            }
            this.clickSubcribors.Add(OnSelected);
        }


        /// <summary>
        /// Event fired when clicked a choice.
        /// OJU: The event clears the list <see cref="clickSubcribors"/>
        /// because when a choice is made, the app removes all the 
        /// choices and so, we don't need the old subscribors 
        /// to this event. Therefore, the subscribors doesn't need
        /// to unsubscribe of the event
        /// </summary>
        /// <param name="choice"></param>
        private void OnClickChoice(Choice choice)
        {
            for (int i = 0; i < this.choices.Length; i++)
            {
                this.container.RemoveLastControl();                
                this.choices[i] = null;
            }

            this.lastMessage = this.container.GetChildOrNull<Message>(this.container.GetChildCount() - 1);

            for (int i = 0; i < this.clickSubcribors.Count; i++)
            {
                this.clickSubcribors[i].Invoke(choice);
            }

            // when we click on some choice, 
            // clear the event as we don't need 
            // the choices anymore
            this.clickSubcribors.Clear();
        }

        /// <summary>
        /// Event fired when <see cref="WriteMessageWaitingTime(in string, bool, float)"/>
        /// consumes the waiting time.
        /// </summary>
        private void OnTime()
        {
            MyConsole.Write("On time");
            this.container.RemoveLastControl();
            this.lastMessage = this.container.GetChildOrNull<Message>(this.container.GetChildCount() - 1);
            this.WriteMessage(toWrite.text, toWrite.isRight);
            controller.OnTime();
            timer.Stop();
        }
    }
}
using Godot;
using System;
using MySystems;
using Base.Interfaces;
using System.Collections.Generic;

namespace Apps.Messages
{
    
    public class ChatsController : System_Base
    {
        /// <summary>
        /// The view attached to this controller.
        /// At some point we're gonna change this to be a
        /// dictionary
        /// </summary>
        private ChatView view;
        
        /// <summary>
        /// List of subscriptors called when
        /// <see cref="OnTime"/> is called
        /// </summary>
        /// <returns></returns>
        private List<Action<int>> toTime = new List<Action<int>>();

        public StyleBoxFlat NormalStyle {get; private set;}
        public StyleBoxFlat SelectedStyle {get; private set;}


        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="view">The view attached. TODO: change</param>
        public ChatsController(in ChatView view){
            this.view = view;
        }

        ///
        public override void OnEnterSystem(params object[] obj)
        {
            Godot.Messages.EnterSystem(this);
            this.NormalStyle = GD.Load<StyleBoxFlat>("res://Resources/Messages_Styles/BasicMessageStyle.tres");

            this.SelectedStyle = GD.Load<StyleBoxFlat>("res://Resources/Messages_Styles/SelectedMessageStyle.tres");
        }

        public override void OnExitSystem(params object[] obj)
        {
            Godot.Messages.ExitSystem(this);
        }

        /// <summary>
        /// Sends a message to the view
        /// </summary>
        /// <param name="text">Text to send</param>
        /// <param name="isRight">Align to the right?</param>
        public void SendMessage(in string text, bool isRight){
            view.WriteMessage(text, isRight);
        }

        /// <summary>
        /// Sends a pre-message (..., "writing", etc) 
        /// to the view during <paramref name="time"/> seconds. 
        /// When time has passed, writes the message
        /// </summary>
        /// <param name="text">Text to send</param>
        /// <param name="isRight">Align to the riht?</param>
        /// <param name="time">Time to wait to print the message</param>
        public void SendMessageWaitingTime(in string text, bool isRight, float time){
            if(time == 0){
                view.WriteMessage(text, isRight);
                return;
            }

            view.WriteMessageWaitingTime(text, isRight, time);
        }

        /// <summary>
        /// Send choices to the view.
        /// </summary>
        /// <param name="choicesText">Array of the choices text</param>
        /// <param name="OnSelected">Event called when a choice is selected</param>
        public void SendChoices(string[] choicesText, Action<Choice> OnSelected){
            this.view.WriteChoices(choicesText, OnSelected);
        }

        
        /// <summary>
        /// Subscribes to a time event.
        /// This event is only used on <see cref="SendMessageWaitingTime(in string, bool, float)"/>
        /// </summary>
        /// <param name="node">The event to launch</param>
        public void SubscribeToTime(Action<int> node){
            this.toTime.Add(node);
        }

        /// <summary>
        /// unSubscribes from a time event.
        /// This event is only used on <see cref="SendMessageWaitingTime(in string, bool, float)"/>
        /// </summary>
        /// <param name="node">The event to remove</param>
        public void UnsubscriteToTime(Action<int> node){
            this.toTime.Remove(node);
        }

        /// <summary>
        /// Event called when <see cref="SendMessageWaiting"/> 
        /// consumes the time
        /// </summary>
        public void OnTime(){
            for(int i = 0; i < this.toTime.Count; i++){
                this.toTime[i].Invoke(1);
            }
        }

    }
}


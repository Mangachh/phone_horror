using Godot;
using MySystems;
using System;
using System.Collections.Generic;

namespace Apps.Messages
{
    /// <summary>
    /// Class for the choice
    /// </summary>
    public class Choice : Message
    {
        // TODO: modify into a file
        private StyleBoxFlat normalStyle;
        private StyleBoxFlat selectedStyle;

        /// <summary>
        /// The index of this choice
        /// </summary>
        public int Index{get;set;}

        /// <summary>
        /// List of subscribors called when <see cref="OnClick"/> is raised
        /// </summary>
        private List<Action<Choice>> onClick;

        public override void _EnterTree()
        {
            base._EnterTree();

            // connect signals
            base.Connect("mouse_entered", this, nameof(OnMouseEnter));
            base.Connect("mouse_exited", this, nameof(OnMouseExit));

            // get styles
            ChatsController controller;
            if(SystemManager.GetInstance(this).TryGetSystem<ChatsController>(out controller)){
                this.normalStyle = controller.NormalStyle;
                this.selectedStyle = controller.SelectedStyle;
            }else{
                MyConsole.WriteError("Choices didn't find a ChatsController");
            }            

            // new list
            this.onClick = new List<Action<Choice>>();
        }

        /// <summary>
        /// Event checker that raises <see cref="OnClick"/>
        /// if the player clicks this choice
        /// </summary>
        public override void _GuiInput(InputEvent @event)
        {
            if(@event is InputEventMouseButton eventButton){
                if(eventButton.ButtonIndex == 1 && eventButton.IsPressed()){
                    MyConsole.Write($"Presionado tio: {this.Text}");
                    this.OnClick();
                }
            }
        }

        /// <summary>
        /// On click event
        /// </summary>
        private void OnClick(){
            for(int i = 0; i < this.onClick.Count; i++){
                this.onClick[i].Invoke(this);
            }
        }        

        /// <summary>
        /// Event raised when the mouse enters this choice.
        /// </summary>
        private void OnMouseEnter(){
            MyConsole.Write("Mouse enter");
            //base.background.Set("custom_styles/panel", this.style);
            this.AddStyleboxOverride("panel", this.selectedStyle);
        }

        /// <summary>
        /// Event raised when the mouse exits this choice
        /// </summary>
        private void OnMouseExit(){
            //base.background.Set("custom_styles/panel/bg_color", this.normalColor);
            MyConsole.Write("Mouse exit");
            this.AddStyleboxOverride("panel", this.normalStyle);
        }

        /// <summary>
        /// Subscribes to <see cref="OnClick"/>
        /// </summary>
        /// <param name="choice">Method to raise</param>
        public void SubscribeOnClick(Action<Choice> choice){
            this.onClick.Add(choice);
        }

        /// <summary>
        /// Unsubscribes to <see cref="OnClick"/>
        /// </summary>
        /// <param name="choice">Method to remove</param>
        public void UnsubscribeOnClick(Action<Choice> choice){
            this.onClick.Remove(choice);
        }


    }
}


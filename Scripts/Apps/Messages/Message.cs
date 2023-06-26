using Godot;
using System;

namespace Apps.Messages
{
    /// <summary>
    /// Basic class for the messages in the app
    /// </summary>
    public class Message : Container
    {
        /// <summary>
        /// Label with the text
        /// </summary>
        protected Label label;

        /// <summary>
        /// The bacgrkound of the text
        /// </summary>
        protected PanelContainer background;

        [Export]
        private readonly string textName;

        [Export]
        private readonly string panelName;


        /// <summary>
        /// Property with the message's text
        /// </summary>
        /// <value>the message's text</value>
        public string Text{
            get => label.Text;
        }

        public override void _EnterTree()
        {
            this.label = base.GetNode<Label>(String.Concat(this.textName));

            MyConsole.Write(this.label);

        }       

        /// <summary>
        /// Writes a message
        /// </summary>
        /// <param name="text">Message to write</param>
        public void WriteMessage(in string text, int maxWidth)
        {
            this.label.Text = text;
            this.CallDeferred("Resize", maxWidth);
        }       

        /// <summary>
        /// Resizes the label and the panel as needed.
        /// 
        /// </summary>
        private void Resize(int maxWidth)
        {
            if(this.label.GetRect().Size.x > maxWidth){
                MyConsole.Write("Inside");
                this.label.Autowrap = true;
                this.label.RectMinSize = new Vector2(maxWidth, this.label.RectSize.y);                                           
            }       

            base.PropagateNotification(NotificationVisibilityChanged);
        }        
    }
}


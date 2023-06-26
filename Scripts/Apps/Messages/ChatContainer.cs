using Godot;
using System;
using System.Collections.Generic;


namespace Apps.Messages
{

    /// <summary>
    /// The container of the chat
    /// </summary>
    public class ChatContainer : Container
    {
        /// <summary>
        /// Separation between messages.
        /// Instead of using a <see cref="VBoxContainer"/> we use
        /// our custom solution. 
        /// The truth is, I can't manage to make the vbox work
        /// </summary>
        [Export]
        private int separation = 10;

        /// <summary>
        /// The last control rendered
        /// </summary>
        private Control lastControl;

        /// <summary>
        /// Subscribor for the <see cref="OnAddChild(Message)"/> event
        /// </summary>
        private HashSet<Action<Message>> onAddChildSubs;


        public override void _EnterTree()
        {
            // poner que el RectMinSizeX sea el del padre
            this.onAddChildSubs = new HashSet<Action<Message>>();

        }

        /// <summary>
        /// Resizes the control asyncroniosly because
        /// we need to wait for the next frame to make
        /// <see cref="Scroller"/> work on focus
        /// </summary>
        /// <param name="control">The control to resize</param>
        /// <param name="isRight">Is it right aligned?</param>
        /// <param name="previous">Previos control, used usually with coices</param>
        /// <returns></returns>
        public async void PlaceControl(Control control, bool isRight, Control previous)
        {
            if (previous == null)
            {
                if (isRight)
                {
                    control.RectPosition = new Vector2(this.RightPosition(control), control.MarginTop);
                }
                else
                {
                    control.RectPosition = new Vector2(control.MarginLeft, control.MarginTop);
                }

            }
            else
            {
                if (isRight)
                {
                    control.RectPosition = new Vector2(this.RightPosition(control), this.YPosition(control, previous));
                }
                else
                {
                    control.RectPosition = new Vector2(control.MarginLeft, this.YPosition(control, previous));
                }

            }
            this.lastControl = control;
            this.RectMinSize = new Vector2(this.RectMinSize.x, control.RectPosition.y +
                                                               control.RectSize.y);

            GD.Print(this.lastControl.RectSize);
            
            // waits to grab focus to work with the scroller
            await ToSignal(base.GetTree(), "idle_frame");
            lastControl.GrabFocus();
        }


        private float RightPosition(in Control control){
            // mmmmm, falla la posición esta...
            MyConsole.Write("Rect size: " + control.RectSize);
            return this.RectMinSize.x - control.MarginRight - control.RectSize.x;
            //return this.RectMinSize.x - 150;
        }

        private float YPosition(in Control control, in Control last){
            return last.RectSize.y + last.RectPosition.y + this.separation;
        }


        // events, used on add child, UNUSED
        private void OnAddChild(Message message){
            if(this.onAddChildSubs.Count != 0){
                MyConsole.WriteWarning("Inside OnAddChild from AppContainer");
                foreach(Action<Message> func in this.onAddChildSubs){
                    func.Invoke(message);
                }                  
            }
        }

        public void SubscribeToAddChild(Action<Message> func){
            this.onAddChildSubs.Add(func);
        }

        public void UnSubscribeToAddChild(Action<Message> func){
            this.onAddChildSubs.Remove(func);
        }

        public void RemoveLastControl(){
            
            try{
                Node last = base.GetChild(base.GetChildCount() - 1);
                base.RemoveChild(last);
                last.QueueFree();
            }catch(Exception e){

            }
        }
    }
}


using Godot;
using System;

namespace Apps.Messages
{   
    [Obsolete]
    public class Scroller : ScrollContainer
    {
        private ChatContainer container;
        private VScrollBar bar;

        public override void _Ready()
        {
            this.getBar();
            this.getContainer();
        }

        private void getBar(){
            Node temp;
            for(int i = 0; i < base.GetChildCount(); i++){
                temp = base.GetChild(i);
                if((bar = temp as VScrollBar) != null){
                    return;
                }
            }
        }

        private void getContainer(){
            Node temp;
            for(int i = 0; i < base.GetChildCount(); i++){
                temp = base.GetChild(i);

                if((container = temp as ChatContainer) != null){
                    this.container.SubscribeToAddChild(OnAddMessage);
                    return;
                }
            }
        }


        private async void OnAddMessage(Message message){
            MyConsole.WriteWarning("OnAddMessage from Scroller");
            //float difference = this.container.RectSize.y - message.RectSize.y;
            float difference = (base.RectSize.y) - (message.RectPosition.y + message.RectSize.y);
            MyConsole.Write("Scroller Size: " + this.container.RectSize.y.ToString());
            MyConsole.Write("Message Position: " + message.RectPosition.y.ToString());
            MyConsole.Write("Message Size: " + message.RectSize.y.ToString());
            MyConsole.Write("Difference: " + difference.ToString());
            
            
            if(difference < 0){
                MyConsole.Write("Difference < 0");
                //base.CallDeferred("set_v_scroll", difference);
                //base.Hide();
                //this.bar.Value = -difference;
                await ToSignal(base.GetTree(), "idle_frame");
                base.ScrollVertical += -(int)difference;                
                //base.CallDeferred(nameof(Resize), -(int)difference);
                
            }
        }
        
        private void Resize(int difference){
            base.Show();
            base.Update();
        }

    }
}



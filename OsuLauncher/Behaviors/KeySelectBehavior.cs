using HandyControl.Interactivity;
using TextBox = HandyControl.Controls.TextBox;

namespace OsuLauncher.Behaviors;

public class KeySelectBehavior : Behavior<TextBox>
{
    protected override void OnAttached()
    {
        AssociatedObject.Clear();
    }
}
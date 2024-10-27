using Microsoft.AspNetCore.Components.Web;

namespace AppServer.Infrastructure.Razor;

public class PspMouseEventArgs : MouseEventArgs
{
    public int Count
    {
        get;
        set;
    }
    public PspMouseEventArgs(MouseEventArgs args, int count)
    {
        ShiftKey = args.ShiftKey;
        CtrlKey = args.CtrlKey;
        MetaKey = args.MetaKey;
        AltKey = args.AltKey;
        Detail = args.Detail;
        Type = args.Type;

        Buttons = args.Buttons;
        Button = args.Button;

        ClientX = args.ClientX;
        ClientY = args.ClientY;

        MovementX = args.MovementX;
        MovementY = args.MovementY;

        OffsetX = args.OffsetX;
        OffsetY = args.OffsetY;

        PageX = args.PageX;
        PageY = args.PageY;

        ScreenX = args.ScreenX;
        ScreenY = args.ScreenY;

        Count = count;
    }
}
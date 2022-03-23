using System.Windows.Controls;
using System.Windows.Shapes;

namespace PingPong
{
    public class Object
    {
        public double Top { get; set; }
        public double Left { get; set; }
        public double Bottom { get; set; }
        public double Right { get; set; }

        public Object(Rectangle _object)
        {
            Top = Canvas.GetTop(_object);
            Left = Canvas.GetLeft(_object);
            Bottom = Top + _object.ActualHeight;
            Right = Left + _object.ActualWidth;
        }
    }
}
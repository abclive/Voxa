using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using OpenTK.Graphics;

namespace Voxa.Objects
{
    public class DebugLine
    {
        public Vector3 Start;
        public Vector3 End;
        public Color4  Color;

        public DebugLine(Vector3 start, Vector3 end)
        {
            this.Start = start;
            this.End = end;
            this.Color = Color4.Red;
        }

        public DebugLine(Vector3 start, Vector3 end, Color4 color)
        {
            this.Start = start;
            this.End = end;
            this.Color = color;
        }
    }
}

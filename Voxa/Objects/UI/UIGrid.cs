using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using System.Drawing;

namespace Voxa.Objects.UI
{
    public class UIGrid : UIElement
    {
        public class UIColumn : UIElement, ICanvas
        {
            public Size Size { get; private set; }

            public UIColumn(Vector2 position, Size size)
            {
                this.Position = position;
                this.Size = size;
            }

            public void SetSize(Size newSize)
            {
                this.Size = newSize;
                foreach (UIElement element in this.Children) {
                    element.SetDirty();
                }
            }

            public void SetPosition(Vector2 newPos)
            {
                this.Position = newPos;
                foreach (UIElement element in this.Children) {
                    element.SetDirty();
                }
            }
        }

        public int ColumnCount { get; private set; }
        public Size Size { get; private set; }
        public List<UIColumn> Columns { get; private set; }

        public UIGrid(Vector2 position, Size size, int columnCount)
        {
            this.Position = position;
            this.Size = size;
            this.ColumnCount = columnCount;
            this.Columns = new List<UIColumn>();

            int colW = this.Size.Width / this.ColumnCount;
            int colH = this.Size.Height;

            for (int i = 0; i < this.ColumnCount; i++) {
                float colPosX = this.Position.X + (colW * i);
                float colPosY = this.Position.Y;

                UIColumn column = new UIColumn(new Vector2(colPosX, colPosY), new Size(colW, colH));
                column.SetParent(this);
                this.Columns.Add(column);
            }
        }

        public void SetSize(Size size)
        {
            this.Size = size;
            int colW = this.Size.Width / this.ColumnCount;
            int colH = this.Size.Height;
            for (int i = 0; i < this.ColumnCount; i++) {
                float colPosX = this.Position.X + (colW * i);
                float colPosY = this.Position.Y;

                this.Columns[i].SetPosition(new Vector2(colPosX, colPosY));
                this.Columns[i].SetSize(new Size(colW, colH));
            }
        }

        public void SetPosition(Vector2 newPos)
        {
            this.Position = newPos;
            int colW = this.Size.Width / this.ColumnCount;

            for (int i = 0; i < this.ColumnCount; i++) {
                float colPosX = this.Position.X + (colW * i);
                float colPosY = this.Position.Y;

                this.Columns[i].SetPosition(new Vector2(colPosX, colPosY));
            }
        }
    }
}

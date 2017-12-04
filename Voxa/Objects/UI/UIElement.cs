using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using OpenTK;

namespace Voxa.Objects.UI
{
    public class UIElement
    {
        public Vector2 Position { get; protected set; }
        public UIElement Parent { get; protected set; }
        public List<UIElement> Children { get; protected set; }
        public OrthographicCamera OrthoCamera { get; protected set; }

        protected bool isDirty = false;

        public UIElement()
        {
            this.OrthoCamera = Engine.RenderingPool.UICamera;
            this.Children = new List<UIElement>();
        }

        public virtual void SetOrthoCamera(OrthographicCamera camera)
        {
            this.OrthoCamera = camera;
        }

        public virtual void SetDirty()
        {
            this.isDirty = true;
        }

        public void AddChild(UIElement child)
        {
            this.Children.Add(child);
        }

        public void RemoveChild(UIElement child)
        {
            this.Children.Remove(child);
        }

        public void RemoveParent()
        {
            if (this.Parent != null) {
                this.Parent.RemoveChild(this);
                this.Parent = null;
            }
        }

        public void SetParent(UIElement parent)
        {
            if (this.Parent != null) {
                this.RemoveParent();
            }
            this.Parent = parent;
            parent.AddChild(this);
        }
    }
}

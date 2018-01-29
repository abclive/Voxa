using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Input;
using Voxa.Objects.Renderer;

namespace Voxa.Objects.UI
{
    public class UIButton : UIElement, ICanvas
    {
        public enum ButtonState
        {
            Idle,
            Hovered,
            Selected,
            Clicked
        }

        public Color4      IdleColor;
        public Color4      HoverColor;
        public Color4      ClickColor;

        public ButtonState State { get; private set; }
        public UIText      Text { get; private set; }
        public UISprite    BackgroundSprite { get; private set; }
        public Size        Size { get; private set; }

        public delegate void ButtonEvent();
        public event ButtonEvent ButtonClicked;

        private GameObject linkedObject;

        //private List<DebugLine> debugLines;
        //private LineRenderer lineRenderer;

        public UIButton(Vector2 position, Size size, string text, Color4 color, Rendering.Font font, GameObject linkedObject, Sprite backgroundSprite = null)
        {
            this.Position = position;
            this.Size = size;
            this.linkedObject = linkedObject;
            this.IdleColor = color;
            this.Text = new UIText(this, UIText.AlignmentMode.Center, text, color, font, 1, linkedObject);
            if (backgroundSprite != null) {
                this.BackgroundSprite = new UISprite(this, backgroundSprite, 1, UISprite.AlignmentMode.Center, linkedObject);
                this.BackgroundSprite.SetRescaleMode(UISprite.ScaleMode.Fill);
            }

            Engine.EngineWindow.MouseMove += onMouseMoved;
            Engine.EngineWindow.Mouse.ButtonDown += onMouseButtonDown;
            Engine.EngineWindow.Mouse.ButtonUp += onMouseButtonUp;

            // DEBUG
            //this.debugLines = new List<DebugLine>();
            //this.debugLines.Add(new DebugLine(new Vector3(this.Position), new Vector3(this.Position.X + size.Width, this.Position.Y, 0), Color4.Red));
            //this.debugLines.Add(new DebugLine(new Vector3(this.Position.X + size.Width, this.Position.Y, 0), new Vector3(this.Position.X + size.Width, this.Position.Y + size.Height, 0), Color4.Red));
            //this.debugLines.Add(new DebugLine(new Vector3(this.Position.X + size.Width, this.Position.Y + size.Height, 0), new Vector3(this.Position.X, this.Position.Y + size.Height, 0), Color4.Red));
            //this.debugLines.Add(new DebugLine(new Vector3(this.Position.X, this.Position.Y + size.Height, 0), new Vector3(this.Position.X, this.Position.Y, 0), Color4.Red));
            //this.lineRenderer = new LineRenderer(this.debugLines);
            //this.linkedObject.AttachComponent(this.lineRenderer);
            //this.lineRenderer.Init();
        }

        private void onMouseButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (e.Button == MouseButton.Left) {
                int yPos = Engine.EngineWindow.Height - e.Y;
                if ((e.X > this.Position.X && e.X < this.Position.X + this.Size.Width) && (yPos > this.Position.Y && yPos < this.Position.Y + this.Size.Height)) {
                    this.ChangeState(ButtonState.Hovered);
                } else {
                    this.ChangeState(ButtonState.Idle);
                }
            }
        }

        private void onMouseButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.Button == MouseButton.Left) {
                if (this.State == ButtonState.Hovered) {
                    this.ChangeState(ButtonState.Clicked);
                }
            }
        }

        private void onMouseMoved(object sender, MouseMoveEventArgs e)
        {
            if (this.State != ButtonState.Clicked) {
                int yPos = Engine.EngineWindow.Height - e.Y;
                if ((e.X > this.Position.X && e.X < this.Position.X + this.Size.Width) && (yPos > this.Position.Y && yPos < this.Position.Y + this.Size.Height)) {
                    this.ChangeState(ButtonState.Hovered);
                } else {
                    this.ChangeState(ButtonState.Idle);
                }
            }
        }

        public void ChangeState(ButtonState newState)
        {
            if (this.State != newState) {
                if (newState == ButtonState.Idle) {
                    //foreach (DebugLine line in this.debugLines) {
                    //    line.Color = Color4.Red;
                    //}
                    //this.lineRenderer.UpdateVertexBuffer();
                    this.Text.SetColor(this.IdleColor);
                } else if (newState == ButtonState.Hovered) {
                    //foreach (DebugLine line in this.debugLines) {
                    //    line.Color = Color4.LightYellow;
                    //}
                    //this.lineRenderer.UpdateVertexBuffer();
                    this.Text.SetColor(this.HoverColor);
                } else if (newState == ButtonState.Clicked) {
                    //foreach (DebugLine line in this.debugLines) {
                    //    line.Color = Color4.Yellow;
                    //}
                    //this.lineRenderer.UpdateVertexBuffer();
                    this.Text.SetColor(this.ClickColor);
                    if (this.ButtonClicked != null) {
                        this.ButtonClicked();
                    }
                }
                this.State = newState;
            }
        }

        public void Destroy()
        {
            this.ButtonClicked = null;
            Engine.EngineWindow.Mouse.ButtonDown -= this.onMouseButtonDown;
            Engine.EngineWindow.Mouse.ButtonUp -= this.onMouseButtonUp;
            Engine.EngineWindow.MouseMove -= this.onMouseMoved;
            this.BackgroundSprite.Destroy();
            this.Text.Destroy();
            this.BackgroundSprite = null;
            this.Text = null;
        }
    }
}

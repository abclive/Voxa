using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;
using Voxa.Rendering.Uniforms;
using Voxa.Utils;

namespace Voxa.Rendering
{
    public sealed class EngineWindow : GameWindow
    {
        public bool LockMouse { get { return this.lockMouse; } set { this.CursorVisible = !value; this.lockMouse = value; } }
        public Vector2 LastMousePos = new Vector2();

        private bool lockMouse;

        public EngineWindow(int windowWidth, int windowHeight, bool fullscreen = false, string windowTitle = "Voxa", bool lockMouse = true) : base(windowWidth, windowHeight, new GraphicsMode(32, 1, 0, 4), windowTitle, (fullscreen) ? GameWindowFlags.Fullscreen : GameWindowFlags.FixedWindow, DisplayDevice.Default, 3, 0, GraphicsContextFlags.ForwardCompatible)
        {
            this.VSync = VSyncMode.Off;
            Logger.Info("OpenGL version: " + GL.GetString(StringName.Version));
            this.lockMouse = lockMouse;
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            GL.ClearColor(Engine.Game.Config.ClearColor.X, Engine.Game.Config.ClearColor.Y, Engine.Game.Config.ClearColor.Z, Engine.Game.Config.ClearColor.W);

            GL.Enable(EnableCap.Multisample);
            GL.Enable(EnableCap.DepthTest);
            GL.Enable(EnableCap.Texture2D);
            GL.Enable(EnableCap.Blend);
            GL.Enable(EnableCap.DebugOutput);
            GL.Enable(EnableCap.DebugOutputSynchronous);

            GL.PixelStore(PixelStoreParameter.UnpackAlignment, 1);

            GL.DebugMessageCallback(OpenGLDebugger.DebugCallback, (IntPtr)null);

            GL.DepthMask(true);
            GL.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.OneMinusSrcAlpha);

            Engine.WhitePixelTexture = new Texture("Voxa.Assets.Textures.WhitePixel.png");
            Engine.WhitePixelTexture.Filtering = Texture.FilteringMode.NEAREST_NEIGHBOR;
            Engine.WhitePixelTexture.Load();
            Engine.Game.Start();
            this.CursorVisible = !this.LockMouse;
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);

            GL.Viewport(ClientRectangle.X, ClientRectangle.Y, ClientRectangle.Width, ClientRectangle.Height);
        }

        void resetCursor()
        {
            OpenTK.Input.Mouse.SetPosition(Bounds.Left + Bounds.Width / 2, Bounds.Top + Bounds.Height / 2);
            this.LastMousePos = new Vector2(OpenTK.Input.Mouse.GetState().X, OpenTK.Input.Mouse.GetState().Y);
        }

        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            base.OnUpdateFrame(e);

            Engine.TaskQueue.CallPendingCallbacks();
            Engine.Game.Update(e);
            if (this.Focused && this.LockMouse) {
                this.resetCursor();
            }
            

            if (Keyboard[Key.Escape])
                Exit();
        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            base.OnRenderFrame(e);

            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            Engine.Game.Render(e);

            // Reset state for potential further draw calls (optional, but good practice)
            GL.BindVertexArray(0);
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
            GL.UseProgram(0);

            this.SwapBuffers();
        }

        protected override void OnFocusedChanged(EventArgs e)
        {
            base.OnFocusedChanged(e);

            if (this.Focused && this.LockMouse) {
                this.resetCursor();
            }
        }

    }
}

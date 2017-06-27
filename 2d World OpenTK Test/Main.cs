using System;
using OpenTK;
using System.Drawing;
using Objects;
using OpenTK.Graphics;
using OpenTK.Input;
using System.Windows;

namespace Control
{
    public class Game
    {
        GameWindow window;
        public double aspectRatio = 1.77777777778;
        double[] white = new double[3] { 1.0, 1.0, 1.0 };
        Floor floor1;
        int mouseX;
        int mouseY;
        ExitButton exitButton = new ExitButton();
        Ball ball = new Ball(1.0, 25.0, 10.0);
        Ball ball2 = new Ball(1.0, 3.0, 5.0);

        public Game(GameWindow window)
        {
            this.window = window;

            this.VariableInitialization();
        }

        public void VariableInitialization()
        {   
            floor1 = new Floor(50.0, 2.0, 0.0, 45.0, white);
            this.Start();
        }

        public void Start()
        {
            //Main Game Loop
            this.window.Load += onLoaded;
            this.window.Resize += onResize;
            this.window.MouseDown += GetMousePosition;
            this.window.UpdateFrame += Physics;
            this.window.RenderFrame += Render;
            this.window.Run(1.0 / 60.0);
        }
        public void onLoaded(object o, EventArgs e)
        {
            GL.ClearColor(0f, 0f, 0f, 0f);
        }
        public void GetMousePosition(object o, MouseButtonEventArgs e)
        {
            mouseX = e.Mouse.X;
            mouseY = e.Mouse.Y;
        }
        public void onResize(object o, EventArgs e)
        {
            window.WindowState = WindowState.Fullscreen;
            GL.Viewport(0, 0, this.window.Width, this.window.Height);
            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadIdentity();
            GL.Ortho(0.0 * aspectRatio, 50.0 * aspectRatio, 50.0, 0.0, -1.0, 1.0);
            GL.MatrixMode(MatrixMode.Modelview);
        }

        //Physics Manipulation
        public void Physics(object o, EventArgs e)
        {
            exitButton.ifClicked(mouseX, mouseY);
            ball.Gravity(floor1.top);
            floor1.Physics();
            ball2.Gravity(floor1.top);
            if(ball.bottom > floor1.top && ball.left >= floor1.left && ball.right <= floor1.right)
            {
                ball.isHit = true;
            }
            if (ball2.bottom > floor1.top && ball2.left >= floor1.left && ball2.right <= floor1.right)
            {
                ball2.isHit = true;
            }
        }

        //Render
        public void Render(object o, EventArgs e)
        {
            GL.LoadIdentity();
            GL.Clear(ClearBufferMask.ColorBufferBit);

            exitButton.Render();

            floor1.Render();

            ball.Render();
            ball2.Render();

            window.SwapBuffers();
        }
    }
}
using System;
using System.Drawing;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using System.Collections.Generic;
using Control;
using Objects;
using System.Diagnostics;

namespace Objects
{
    class Floor
    {
        public double height, width, x_position, y_position;
        public double[,] vertices = new double[4, 2];
        double[] color;
        double aspectRatio = 1.77777777778;
        public double top, left, right;
        public Floor(double width, double height, double x_position, double y_position, double[] color)
        {
            this.height = height;
            this.width = width;
            this.x_position = x_position;
            this.y_position = y_position;
            this.color = color;
            GetVertices();
        }
        public void GetVertices()
        {
            vertices[0, 0] = (x_position + width) * this.aspectRatio;
            vertices[0, 1] = y_position;

            vertices[1, 0] = x_position * this.aspectRatio;
            vertices[1, 1] = y_position;

            vertices[2, 0] = x_position * this.aspectRatio;
            vertices[2, 1] = y_position + height;

            vertices[3, 0] = (x_position + width) * this.aspectRatio;
            vertices[3, 1] = y_position + height;
        }
        public void Physics()
        {
            this.top = this.y_position;
            this.left = this.x_position * this.aspectRatio;
            this.right = (this.x_position + this.width) * this.aspectRatio;
        }
        public void Render()
        {
            GL.Begin(BeginMode.Quads);

            GL.Color3(color[0], color[1], color[2]);
            GL.Vertex2(vertices[0, 0], vertices[0, 1]);
            GL.Vertex2(vertices[1, 0], vertices[1, 1]);
            GL.Vertex2(vertices[2, 0], vertices[2, 1]);
            GL.Vertex2(vertices[3, 0], vertices[3, 1]);

            GL.End();
        }
    }
    class ExitButton
    {
        int x_position = 48;
        int y_position = 0;
        int height = 2;
        int width = 2;
        double aspectRatio = 1.77777777778;
        public void Render()
        {
            GL.Begin(BeginMode.Quads);
            GL.Color3(1.0, 0.0, 0.0);
            GL.Vertex2(48.0 * this.aspectRatio, 0.0);
            GL.Vertex2(50.0 * this.aspectRatio, 0.0);
            GL.Vertex2(50.0 * this.aspectRatio, 2.0);
            GL.Vertex2(48.0 * this.aspectRatio, 2.0);
            GL.End();

            GL.Begin(BeginMode.Lines);
            GL.Color3(1.0, 1.0, 1.0);
            GL.Vertex2(48.2 * this.aspectRatio, 0.2);
            GL.Vertex2(49.8 * this.aspectRatio, 1.8);
            GL.Vertex2(49.8 * this.aspectRatio, 0.2);
            GL.Vertex2(48.2 * this.aspectRatio, 1.8);
            GL.End();
        }
        public void ifClicked(int mouseX, int mouseY)
        {
            if (mouseX >= 1310 && mouseX <= 1365 && mouseY >= 0 && mouseY <= 30)
            {
                Environment.Exit(-1);
            }
        }
    }
    public class Ball
    {
        double radius, x_position, y_position;
        double aspectRatio = 1.77777777778;
        List<Vector2d> vertices = new List<Vector2d>();
        int sides = 360;
        double heading;
        public double fall_speed = 0.1;
        public double bottom, left, right;
        public bool isHit = false;
        double bounce = 0.0;
        double doubleCount = 0;
        int count = 0;
        double red, green, blue;
        public Ball(double radius, double x_position, double y_position, double red = 1.0, double green = 1.0, double blue = 1.0)
        {
            this.radius = radius;
            this.x_position = x_position * this.aspectRatio;
            this.y_position = y_position * this.aspectRatio;
            this.red = red;
            this.green = green;
            this.blue = blue;
            for (int a = 0; a < 360; a++)
            {
                heading = Math.PI * 2.0;
                this.vertices.Add(new Vector2d(this.x_position + (this.radius * Math.Cos(a * heading / 360)), this.y_position + (this.radius * Math.Sin(a * heading / 360))));
            }
        }
        public void Gravity(double top)
        {
            this.bottom = this.y_position + this.radius;
            this.left = (this.x_position - this.radius) * this.aspectRatio;
            this.right = (this.x_position + this.radius) * this.aspectRatio;
            if (this.isHit == true)
            {
                if (this.count == 0)
                {
                    this.doubleCount++;
                    this.fall_speed -= this.fall_speed * 2.0;
                    this.count++;
                }
                this.fall_speed += 0.01;
                this.y_position += this.fall_speed;
                if (this.fall_speed >= 0.0)
                {
                    this.isHit = false;
                    this.count = 0;
                    if (this.doubleCount != 8)
                    {
                        this.fall_speed = 0.1;
                    }
                    else
                    {
                        this.fall_speed = 0.0;
                        this.y_position = top - this.radius;
                    }
                }
            }
            else
            {
                this.fall_speed += (this.fall_speed / 45.5);
                if (this.fall_speed > 0.7)
                {
                    this.fall_speed = 0.7;
                }
                this.y_position += this.fall_speed;
            }
            if (x_position - this.radius <= 0.0)
            {
                x_position = this.radius;
            }
            if (x_position + this.radius > (50.0 * this.aspectRatio))
            {
                x_position = (50.0 * this.aspectRatio) - this.radius;
            }
            if ((this.y_position + this.radius) >= 50.0)
            {
                this.y_position = 50.0 - this.radius;
                this.isHit = true;
            }
            if ((this.y_position - this.radius) <= 0.0)
            {
                this.y_position = this.radius;
            }

        }
        public void Render()
        {
            int i;
            int triangleAmount = 20; //# of triangles used to draw circle

            //GLfloat radius = 0.8f; //radius
            float twicePi = 2.0f * float.Parse(Math.PI.ToString());

            GL.Begin(BeginMode.TriangleFan);
            GL.Color3(this.red, this.green, this.blue);
            GL.Vertex2(this.x_position, this.y_position); // center of circle
            for (i = 0; i <= triangleAmount; i++)
            {
                GL.Vertex2(
                        this.x_position + (radius * Math.Cos(i * twicePi / triangleAmount)),
                    this.y_position + (radius * Math.Sin(i * twicePi / triangleAmount))
                );
            }
            GL.End();
        }
    }
}

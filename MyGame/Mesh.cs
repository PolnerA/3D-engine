using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameEngine;
using SFML.Graphics;
using SFML.Graphics.Glsl;
using SFML.System;

namespace MyGame
{
    class Mesh:GameObject
    {
        float fTheta=1.0f;

        List<Triangle> triangles= new List<Triangle>();
        Mat4x4 matProj = new Mat4x4();
        public Mesh(Mat4x4 matproj)
        {
            this.matProj = matproj;
        }
        public void AddTriangle(Triangle triangle)
        {
            triangles.Add(triangle);

        }
        public override void Update(Time elapsed)
        {
            Mat4x4 matRotZ= new Mat4x4();
            Mat4x4 matRotX= new Mat4x4();
            fTheta += 1.0f * elapsed.AsSeconds();
            // Rotation Z
            matRotZ.m4x4[0][0]=(float)Math.Cos(fTheta);
            matRotZ.m4x4[0][1] =(float)Math.Sin(fTheta);
            matRotZ.m4x4[1][0] =(float)-Math.Sin(fTheta);
            matRotZ.m4x4[1][1] =(float)Math.Cos(fTheta);
            matRotZ.m4x4[2][2] = 1;
            matRotZ.m4x4[3][3] = 1;
            // Rotation X
            matRotX.m4x4[0][0] = 1;
            matRotX.m4x4[1][1] =(float)Math.Cos(fTheta * 0.5f);
            matRotX.m4x4[1][2] =(float)Math.Sin(fTheta * 0.5f);
            matRotX.m4x4[2][1] =(float)-Math.Sin(fTheta * 0.5f);
            matRotX.m4x4[2][2] =(float)Math.Cos(fTheta * 0.5f);
            matRotX.m4x4[3][3] = 1;
            for (int i=0; i<triangles.Count;i++)
            {             //Problem area ||
                //                       \/
                Triangle triProjected = new Triangle(); Triangle triTranslated; Triangle triRotatedZ = new Triangle(); Triangle triRotatedZX= new Triangle();

                // Rotate in Z-Axis
                triRotatedZ.a = matRotZ.MultiplyMatrixVector(triangles[i].a);
                triRotatedZ.b = matRotZ.MultiplyMatrixVector(triangles[i].b);//matrix multiplication for rotation leaves 0 values
                triRotatedZ.c = matRotZ.MultiplyMatrixVector(triangles[i].c);
                //output for mat rot z isn't recorded, keeps the input 0 for the second.
                // Rotate in X-Axis
                triRotatedZX.a = matRotX.MultiplyMatrixVector(triRotatedZ.a);
                triRotatedZX.b = matRotX.MultiplyMatrixVector(triRotatedZ.b);
                triRotatedZX.c = matRotX.MultiplyMatrixVector(triRotatedZ.c);
                // Offset into the screen
                triTranslated = triRotatedZX;
                triTranslated.a.Z = triRotatedZX.a.Z + 3.0f;
                triTranslated.b.Z = triRotatedZX.b.Z + 3.0f;
                triTranslated.c.Z = triRotatedZX.c.Z + 3.0f;

                // Project triangles from 3D --> 2D
                triProjected.a = matProj.MultiplyMatrixVector(triTranslated.a);
                triProjected.b = matProj.MultiplyMatrixVector(triTranslated.b);
                triProjected.c = matProj.MultiplyMatrixVector(triTranslated.c);
                
                // Scale into view
                triProjected.a.X += 1.0f; triProjected.a.Y += 1.0f;
                triProjected.b.X += 1.0f; triProjected.b.Y += 1.0f;
                triProjected.c.X += 1.0f; triProjected.c.Y += 1.0f;
                triProjected.a.X *= 0.5f * Game.RenderWindow.Size.X;
                triProjected.a.Y *= 0.5f * Game.RenderWindow.Size.Y;
                triProjected.b.X *= 0.5f * Game.RenderWindow.Size.X;
                triProjected.b.Y *= 0.5f * Game.RenderWindow.Size.Y;
                triProjected.c.X *= 0.5f * Game.RenderWindow.Size.X;
                triProjected.c.Y *= 0.5f * Game.RenderWindow.Size.Y;

                // Rasterize triangle
                Game.CurrentScene.DrawTriangle((int)triProjected.a.X, (int)triProjected.a.Y, (int)triProjected.b.X, (int)triProjected.b.Y, (int)triProjected.c.X, (int)triProjected.c.Y);
                //only one point scaled into view
            }
        }
    }
}

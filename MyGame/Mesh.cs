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
            Console.WriteLine("T:"+fTheta);
            Console.WriteLine("Cos(T):"+Math.Cos(fTheta));
            // Rotation Z
            matRotZ.AddToMatrix((float)Math.Cos(fTheta),0,0);
            Console.WriteLine(matRotZ.m4x4[0][0]);
            matRotZ.AddToMatrix((float)Math.Sin(fTheta),0,1);
            matRotZ.AddToMatrix((float)-Math.Sin(fTheta),1,0);
            matRotZ.AddToMatrix((float)Math.Cos(fTheta),1,1);
            matRotZ.AddToMatrix(1, 2, 2);
            matRotZ.AddToMatrix(1, 3, 3);
            // Rotation X
            matRotX.AddToMatrix(1,0,0);
            matRotX.AddToMatrix((float)Math.Cos(fTheta * 0.5f), 1, 1);
            matRotX.AddToMatrix((float)Math.Sin(fTheta * 0.5f), 1, 1);
            matRotX.AddToMatrix((float)-Math.Sin(fTheta * 0.5f), 2, 2);
            matRotX.AddToMatrix((float)Math.Cos(fTheta * 0.5f), 2, 2);
            matRotX.AddToMatrix(1, 3, 3);
            for (int i=0; i<triangles.Count;i++)
            {             
                Triangle triProjected = new Triangle(); Triangle triTranslated; Triangle triRotatedZ = new Triangle(); Triangle triRotatedZX= new Triangle();

               // Rotate in Z-Axis
                matRotZ.MultiplyMatrixVector(triangles[i].a, triRotatedZ.a, matRotZ);
                matRotZ.MultiplyMatrixVector(triangles[i].b, triRotatedZ.b, matRotZ);
                matRotZ.MultiplyMatrixVector(triangles[i].c, triRotatedZ.c, matRotZ);

                // Rotate in X-Axis
                matRotX.MultiplyMatrixVector(triRotatedZ.a, triRotatedZX.a, matRotX);
                matRotX.MultiplyMatrixVector(triRotatedZ.b, triRotatedZX.b, matRotX);
                matRotX.MultiplyMatrixVector(triRotatedZ.c, triRotatedZX.c, matRotX);

                // Offset into the screen
                triTranslated = triRotatedZX;
                triTranslated.a.Z = triRotatedZX.a.Z + 3.0f;
                triTranslated.b.Z = triRotatedZX.a.Z + 3.0f;
                triTranslated.c.Z = triRotatedZX.a.Z + 3.0f;

                // Project triangles from 3D --> 2D
                matProj.MultiplyMatrixVector(triTranslated.a, triProjected.a, matProj);
                matProj.MultiplyMatrixVector(triTranslated.b, triProjected.b, matProj);
                matProj.MultiplyMatrixVector(triTranslated.c, triProjected.c, matProj);

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
                Game.CurrentScene.AddToList((int)triProjected.a.X, (int)triProjected.a.Y, (int)triProjected.b.X, (int)triProjected.b.Y, (int)triProjected.c.X, (int)triProjected.c.Y);
            }
            GameScene scene = (GameScene)Game.CurrentScene;
            scene.ClearList();
        }
    }
}

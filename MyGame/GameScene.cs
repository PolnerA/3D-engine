using GameEngine;
using SFML.Audio;
using SFML.System;
using SFML.Window;
using System;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Threading;

namespace MyGame
{
    class GameScene : Scene
    {
        public GameScene()
        {
            //projection matrix is good
            Mat4x4 matproj = new Mat4x4();
            float fNear = 0.1f; 
            float fFar = 1000.0f;
            float fFov = 90.0f;
            float fAspectRatio = (float)Game.RenderWindow.Size.Y/(float)Game.RenderWindow.Size.X;
            float fFovRad = (float)1.0f / (float)Math.Tan(fFov * 0.5f / 180.0f * 3.14159f);
            matproj.m4x4[0][0]=fAspectRatio * fFovRad; //0,0 = 0.5629
            matproj.m4x4[1][1] = fFovRad;//1,1 = 1.00003
            matproj.m4x4[2][2]=fFar /(fFar - fNear);//2,2 =
            matproj.m4x4[3][2]=(-fFar * fNear) /(fFar - fNear);//3,2 = -0.10001000100010001000100010001
            matproj.m4x4[2][3]= 1.0f;//2,3 = 1
            matproj.m4x4[3][3]= 0.0f;//3,3 = 0
            //Triangles are good
            Mesh meshcube = new Mesh(matproj);
            //south
            Triangle triangle1 = new Triangle(new Vector3f(0, 0, 0), new Vector3f(0, 1, 0), new Vector3f(1, 1, 0));
            Triangle triangle2 = new Triangle(new Vector3f(0, 0, 0), new Vector3f(1, 1, 0), new Vector3f(1, 0, 0));
            //east
            Triangle triangle3 = new Triangle(new Vector3f(1, 0, 0), new Vector3f(1, 1, 0), new Vector3f(1, 1, 1));
            Triangle triangle4 = new Triangle(new Vector3f(1, 0, 0), new Vector3f(1, 1, 1), new Vector3f(1, 0, 1));
            //north
            Triangle triangle5 = new Triangle(new Vector3f(1, 0, 1), new Vector3f(1, 1, 1), new Vector3f(0, 1, 1));
            Triangle triangle6 = new Triangle(new Vector3f(1, 0, 1), new Vector3f(0, 1, 1), new Vector3f(0, 0, 1));
            //west
            Triangle triangle7 = new Triangle(new Vector3f(0, 0, 1), new Vector3f(0, 1, 1), new Vector3f(0, 1, 0));
            Triangle triangle8 = new Triangle(new Vector3f(0, 0, 1), new Vector3f(0, 1, 0), new Vector3f(0, 0, 0));
            //top
            Triangle triangle9 = new Triangle(new Vector3f(0, 1, 0), new Vector3f(0, 1, 1), new Vector3f(1, 1, 1));
            Triangle triangle10 = new Triangle(new Vector3f(0, 1, 0), new Vector3f(1, 1, 1), new Vector3f(1, 1, 0));
            //bottom
            Triangle triangle11 = new Triangle(new Vector3f(1, 0, 1), new Vector3f(0, 0, 1), new Vector3f(0, 0, 0));
            Triangle triangle12 = new Triangle(new Vector3f(1, 0, 1), new Vector3f(0, 0, 0), new Vector3f(1, 0, 0));
            meshcube.AddTriangle(triangle1);
            meshcube.AddTriangle(triangle2);
            meshcube.AddTriangle(triangle3);
            meshcube.AddTriangle(triangle4);
            meshcube.AddTriangle(triangle5);
            meshcube.AddTriangle(triangle6);
            meshcube.AddTriangle(triangle7);
            meshcube.AddTriangle(triangle8);
            meshcube.AddTriangle(triangle9);
            meshcube.AddTriangle(triangle10);
            meshcube.AddTriangle(triangle11);
            meshcube.AddTriangle(triangle12);
            AddGameObject(meshcube);
        }
    }
}

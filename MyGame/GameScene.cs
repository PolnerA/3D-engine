using GameEngine;
using SFML.System;
using System;

namespace MyGame
{
    class GameScene : Scene
    {
        public GameScene()
        {
            //random is set up for random number generation
            Random rng = new Random();
            //4x4 matrix created for projection, Mat4x4 is a class with a 2d list which acts as the 4x4 matrix
            Mat4x4 matproj = new Mat4x4();
            //fnear is the distance from the players head to the camera
            float fNear = 0.1f; 

            //ffar is the distance from the players head to the end of the view
            float fFar = 1000.0f;
            //ffov is the field of view
            float fFov = 90.0f;
            //aspect ratio is set to the width of the game's render window / the height. This makes any changes to the renderwindows change the aspect ratio
            float fAspectRatio = (float)Game.RenderWindow.Size.Y/(float)Game.RenderWindow.Size.X;
            //ffovRad is the cotangent of the half of the field of view (bit messier as Math.Tan takes radians so a conversion to radians must be done)
            float fFovRad = (float)1.0f / (float)Math.Tan(fFov * 0.5f / 180.0f * 3.14159f);
            //matrix is populated with the values above.
            matproj.m4x4[0][0]=fAspectRatio * fFovRad; //0,0 = 0.5629 matrix contains the following values with 1080/1920 aspect ratio
            matproj.m4x4[1][1] = fFovRad;//1,1 = 1.00003
            matproj.m4x4[2][2]=fFar /(fFar - fNear);//2,2 =1.0001
            matproj.m4x4[3][2]=(-fFar * fNear) /(fFar - fNear);//3,2 = -0.10001000100010001000100010001
            matproj.m4x4[2][3]= 1.0f;//2,3 = 1
            matproj.m4x4[3][3]= 0.0f;//3,3 = 0
            
            //Triangles for 3d graphical rendering
            //triangles are just 3 vector3f's to allow 3 points in 3d space
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
            //all the triangles get added to the meshcube
            AddGameObject(meshcube);
            //the game object is added to start updating it's condition
            //this allows it to draw and for modifications to be made to it
            
            //pyramid mesh

            Mesh meshPyramid = new Mesh(matproj);
            //bottom
            Triangle ptriangle1 = new Triangle(new Vector3f(1,0,1),new Vector3f(0,0,1), new Vector3f(0,0,0));
            Triangle ptriangle2 = new Triangle(new Vector3f(1, 0, 1), new Vector3f(0, 0, 0), new Vector3f(1, 0, 0));
            //south face
            Triangle ptriangle3 = new Triangle(new Vector3f(0.5f, 1f, 0.5f), new Vector3f(0, 0, 0), new Vector3f(1, 0, 0));
            //west face
            Triangle ptriangle4 = new Triangle(new Vector3f(0.5f, 1f, 0.5f), new Vector3f(0, 0, 0), new Vector3f(0, 0, 1));
            //south face
            Triangle ptriangle5 = new Triangle(new Vector3f(0.5f, 1f, 0.5f), new Vector3f(0, 0, 1), new Vector3f(1, 0, 1));
            //east face
            Triangle ptriangle6 = new Triangle(new Vector3f(0.5f, 1f, 0.5f), new Vector3f(1, 0, 1), new Vector3f(1, 0, 0));
            meshPyramid.AddTriangle(ptriangle1);
            meshPyramid.AddTriangle(ptriangle2);
            meshPyramid.AddTriangle(ptriangle3);
            meshPyramid.AddTriangle(ptriangle4);
            meshPyramid.AddTriangle(ptriangle5);
            meshPyramid.AddTriangle(ptriangle6);
            //AddGameObject(meshPyramid); //add it as game object to see it (mesh draws itself when game objects update)

            //random mesh

            Mesh RandomShape = new Mesh(matproj);
            for (int i = 0; i<rng.Next(20)+1; i++)
            {
                Triangle randomtriangle = new Triangle(new Vector3f((float)rng.NextDouble(), (float)rng.NextDouble(), (float)rng.NextDouble()),new Vector3f((float)rng.NextDouble(), (float)rng.NextDouble(), (float)rng.NextDouble()), new Vector3f((float)rng.NextDouble(), (float)rng.NextDouble(), (float)rng.NextDouble()));
                RandomShape.AddTriangle(randomtriangle);
            }
            //loop is for a random amount of triangles between 1 and 20
            //populated with triangles of random double values typecasted into a float
            //the random triangle is added to the mesh

            //AddGameObject(RandomShape); //add it as a game object to see it (mesh draws itself when game objects update)
        }
    }
}

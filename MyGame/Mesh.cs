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
        public Mesh()
        {
        }
        public void AddTriangle(Triangle triangle)
        {
            triangles.Add(triangle);
        }
        public override void Update(Time elapsed)
        {
            Mat4x4 matRotZ, matRotX;
            fTheta += 1.0f * elapsed.AsMilliseconds();
        }
    }
}

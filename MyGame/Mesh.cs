using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyGame
{
    class Mesh
    {
        List<Triangle> triangles= new List<Triangle>();
        public Mesh()
        {
        }
        public void AddTriangle(Triangle triangle)
        {
            triangles.Add(triangle);
        }
    }
}

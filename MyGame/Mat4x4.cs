using SFML.Graphics.Glsl;
using SFML.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyGame
{
    class Mat4x4
    {
       
        public List<List<float>> m4x4 = new List<List<float>>() { new List<float> { 0,0,0,0},
                                                           new List<float> { 0,0,0,0},
                                                           new List<float> { 0,0,0,0},
                                                           new List<float> { 0,0,0,0} };
        public void MultiplyMatrixVector(Vector3f i, Vector3f o)
        {
            o.X = i.X * m4x4[0][0] + i.Y * m4x4[1][0] + i.Z * m4x4[2][0] + m4x4[3][0];
            o.Y = i.X * m4x4[0][1] + i.Y * m4x4[1][1] + i.Z * m4x4[2][1] + m4x4[3][1];
            o.Z = i.X * m4x4[0][2] + i.Y * m4x4[1][2] + i.Z * m4x4[2][2] + m4x4[3][2];
            float w = i.X * m4x4[0][3] + i.Y * m4x4[1][3] + i.Z * m4x4[2][3] + m4x4[3][3];

            if (w != 0.0f)
            {
                o.X /= w; o.Y /= w; o.Z /= w;
            }
        }
        public void AddToMatrix(float a, int indexi, int indexi2)
        {
            m4x4[indexi][indexi2] = a;
        }
    }
}

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
        public Vector3f MultiplyMatrixVector(Vector3f input)
        {
            Vector3f output = new Vector3f();
            output.X = (input.X * m4x4[0][0]) +( input.Y * m4x4[1][0]) + (input.Z * m4x4[2][0]) + m4x4[3][0];
            output.Y = (input.X * m4x4[0][1]) +( input.Y * m4x4[1][1]) + (input.Z * m4x4[2][1]) + m4x4[3][1];
            output.Z = (input.X * m4x4[0][2]) +( input.Y * m4x4[1][2]) + (input.Z * m4x4[2][2]) + m4x4[3][2];
            float w = input.X * m4x4[0][3] + input.Y * m4x4[1][3] + input.Z * m4x4[2][3] + m4x4[3][3];
            
            if (w != 0.0f)
            {
                output.X /= w; output.Y /= w; output.Z /= w;
            }
            return output;
        }
    }
}

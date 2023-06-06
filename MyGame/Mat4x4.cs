﻿using SFML.Graphics.Glsl;
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
        float[,] m4x4 = new float[,] { { } };

        void MultiplyMatrixVector(Vector3f i, Vector3f o, Mat4x4 m)
        {
            o.X = i.X * m.m4x4[0,0] + i.Y * m.m4x4[1,0] + i.Z * m.m4x4[2,0] + m.m4x4[3,0];
            o.Y = i.X * m.m4x4[0,1] + i.Y * m.m4x4[1,1] + i.Z * m.m4x4[2,1] + m.m4x4[3,1];
            o.Z = i.X * m.m4x4[0,2] + i.Y * m.m4x4[1,2] + i.Z * m.m4x4[2,2] + m.m4x4[3,2];
            float w = i.X * m.m4x4[0,3] + i.Y * m.m4x4[1,3] + i.Z * m.m4x4[2,3] + m.m4x4[3,3];

            if (w != 0.0f)
            {
                o.X /= w; o.Y /= w; o.Z /= w;
            }
        }
    }
}
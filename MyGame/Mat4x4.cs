using SFML.System;
using System.Collections.Generic;

namespace MyGame
{
    class Mat4x4
    {
       
        public List<List<float>> m4x4 = new List<List<float>>() { new List<float> { 0,0,0,0},
                                                           new List<float> { 0,0,0,0},
                                                           new List<float> { 0,0,0,0},
                                                           new List<float> { 0,0,0,0} };
        //on creation of a 4x4 matrix it creates a 2d list of float values that are initialy 0
        //4x4 matrix instead of 3x3 to be able to divide all the values by w 
        public Vector3f MultiplyMatrixVector(Vector3f input)
        {
            Vector3f output = new Vector3f(); 
            //first output is asigned to the value of the input x, y and z multiplied by the first row of the matrix, 4th value added on
            output.X = (input.X * m4x4[0][0]) +( input.Y * m4x4[1][0]) + (input.Z * m4x4[2][0]) + m4x4[3][0];
            //second output is asigned to the value of the input x, y and z multiplied by the first row of the matrix, 4th value added on
            output.Y = (input.X * m4x4[0][1]) +( input.Y * m4x4[1][1]) + (input.Z * m4x4[2][1]) + m4x4[3][1];
            //same with the third output
            output.Z = (input.X * m4x4[0][2]) +( input.Y * m4x4[1][2]) + (input.Z * m4x4[2][2]) + m4x4[3][2];
            //last row multiplied by the input with the 4th value added is put into w, to divide by all our output values
            float w = input.X * m4x4[0][3] + input.Y * m4x4[1][3] + input.Z * m4x4[2][3] + m4x4[3][3];
            
            if (w != 0.0f)//don't divide by 0
            {
                output.X /= w; output.Y /= w; output.Z /= w;
                //divide all of our output by w
            }
            //returns the output
            return output;
        }
    }
}

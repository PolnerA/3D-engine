using SFML.System;

namespace MyGame
{
    class Triangle
    {
        //3 points a b and c each in 3d space as a vector3f
        public Vector3f a;
        public Vector3f b;
        public Vector3f c;
        public Triangle(Vector3f A, Vector3f B, Vector3f C)//when created it populates the triangle with the 3 points
        { 
            a=A; b=B; c=C;
        }
        public Triangle()
        {//if no argument triangle is populated with 0's (can be changed throughout the code as a, b, and c are all public)
            a= new Vector3f(0, 0, 0); b=new Vector3f(0, 0, 0); c=new Vector3f(0,0,0);
        }
    }
}

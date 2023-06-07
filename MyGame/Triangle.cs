using SFML.System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyGame
{
    class Triangle
    {
        public Vector3f a;
        public Vector3f b;
        public Vector3f c;
        public Triangle(Vector3f A, Vector3f B, Vector3f C)
        { 
            a=A; b=B; c=C;
        }
        public Triangle()
        {
            a= new Vector3f(0, 0, 0); b=new Vector3f(0, 0, 0); c=new Vector3f(0,0,0);
        }
        public void ChangeA(Vector3f A) { a=A; }
        public void ChangeB(Vector3f B) { b=B; }
        public void ChangeC(Vector3f C) { c=C; }
    }
}

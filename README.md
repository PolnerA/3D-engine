# 3D Game demo with the Williams Game Engine

This repository is a transformation of the Williams Game engine from a simple 2d based game engine to one which has 3d graphics, 
this is done with multiple matrices the main one matproj is used to change the vectors of 3 points to 2d coordinates, 
it can be used to create functional 3d graphics using SFML and the majority of the code left from the Williams Game Engine.

## Additional details

You can change the shape that's desplayed by adding new triangles for a different mesh that's not a cube, 
for example you can make a rectangle or a pyramid, This is just a simple demonstration of 3d graphics and how they work. 
It can be presented a lot simpler, but all you need to put something 3d is to create a mesh, populate it with triangles, 
and add it as a gameobject. This currently will cause the gameobject to rotate to showcase the 3d graphics as that's in the mesh class.
And if you want the different 3d objects to have different properties simply create a new class which has the same properties as the mesh class, and modify it to your hearts content.
Just remember to multiply the points by the projection matrix (matproj) to be able to have the points shown as 2d.

## Credits

SFML-based game engine was originally written in C++ by Eric Williams. Ported to C# by [Mike Magruder](https://github.com/mikemag). 
Code for the 3d Demo was mostly from [this 3d engine demo](https://github.com/OneLoneCoder/Javidx9/blob/master/ConsoleGameEngine/BiggerProjects/Engine3D/OneLoneCoder_olcEngine3D_Part1.cpp)
This repo is almost the same ported from C++ to C#.

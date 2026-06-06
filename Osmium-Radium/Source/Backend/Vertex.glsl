#version 330 core

layout (location = 0) in vec2 pos;
layout (location = 1) in vec2 uv;
//layout (location = 1) in vec2 uv; todo: UV might be needed, but if i can i would like to merge that in with pos.

uniform float z;
//todo: organize Z from 0 to 100

out vec2 pos_out;
out vec2 uvcoord;

void main()
{
    uvcoord = uv;
    pos_out = pos;
    gl_Position = vec4((pos.x - 50) / 50, ((100 - pos.y) - 50) / 50, z, 1);
}
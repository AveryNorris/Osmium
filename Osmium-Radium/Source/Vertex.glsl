#version 330 core

layout (location = 0) in vec2 pos;
layout (location = 1) in vec2 uv;
//layout (location = 1) in vec2 uv; todo: UV might be needed, but if i can i would like to merge that in with pos.

out vec2 uvcoord;

void main()
{
    uvcoord = uv;
    gl_Position = vec4((pos.x - 50) / 50, ((100 - pos.y) - 50) / 50, 0, 1);
}
#version 330 core

uniform vec4 color;
uniform sampler2D tex;

in vec2 uvcoord;

out vec4 finalColor;

void main() {
    finalColor = texture(tex, uvcoord) * color;
}
#version 330 core

uniform vec4 color;
uniform sampler2D tex;

uniform vec4 clippingRect;

in vec2 uvcoord;
in vec2 pos_out;

out vec4 finalColor;

void main() {
    if(pos_out.x >= clippingRect.x && pos_out.y >= clippingRect.y && pos_out.x <= clippingRect.z && pos_out.y <= clippingRect.w) {
        finalColor = texture(tex, uvcoord) * color;
    }else {
        finalColor = vec4(0, 0, 0, 0);
    }
}
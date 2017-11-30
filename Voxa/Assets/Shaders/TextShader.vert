#version 330 core

in vec2 vPosition;
in vec2 vTexCoord;
out vec2 fTexCoord;

uniform mat4 textProjection;

void main()
{
    gl_Position = textProjection * vec4(vPosition.xy, 0.0, 1.0);
    fTexCoord = vTexCoord;
} 
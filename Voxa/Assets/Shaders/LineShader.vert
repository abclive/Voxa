#version 330 core

uniform mat4 cameraProjection;

// vertex attributes
in vec3 vPosition;
in vec4 vColor;

out vec4 fColor;

void main()
{
    gl_Position = cameraProjection * vec4(vPosition, 1.0);
    fColor = vColor;
}
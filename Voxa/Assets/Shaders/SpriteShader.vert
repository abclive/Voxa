#version 330 core

uniform mat4 cameraProjection;

// vertex attributes
in vec3 vPosition;
in vec4 vColor;
in vec2 vTexCoord;

out vec3 fPosition;
out vec4 fColor;
out vec2 fTexCoord;

void main()
{
    gl_Position = cameraProjection * vec4(vPosition, 1.0);
	fPosition = vPosition;
    fColor = vColor;
	fTexCoord = vTexCoord;
}
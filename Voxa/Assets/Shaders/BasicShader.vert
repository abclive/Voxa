#version 330 core

uniform mat4 modelMatrix;
uniform mat4 projectionMatrix;
uniform mat3 normalMatrix;

// attributes of our vertex
in vec3 vPosition;
in vec4 vColor;
in vec2 vTexCoord;
in vec3 vNormal;

out vec3 fPosition;
out vec4 fColor;
out vec2 fTexCoord;
out vec3 fNormal;

void main()
{
    gl_Position = projectionMatrix * modelMatrix * vec4(vPosition, 1.0);
	fPosition = vPosition;
    fColor = vColor;
	fTexCoord = vTexCoord;
	fNormal = normalMatrix * vNormal;
}
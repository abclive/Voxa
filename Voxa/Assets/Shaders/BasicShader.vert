#version 330 core

// a projection transformation to apply to the vertex' position
uniform mat4 projectionMatrix;
uniform mat3 normalMatrix;

// attributes of our vertex
in vec3 vPosition;
in vec4 vColor;
in vec2 vTexCoord;
in vec3 vNormal;

out vec3 fPosition;
out vec4 fColor; // must match name in fragment shader
out vec2 fTexCoord;
out vec3 fNormal;

void main()
{
    // gl_Position is a special variable of OpenGL that must be set
    gl_Position = projectionMatrix * vec4(vPosition, 1.0);
	fPosition = vPosition;
    fColor = vColor;
	fTexCoord = vTexCoord;
	fNormal = normalMatrix * vNormal;
}
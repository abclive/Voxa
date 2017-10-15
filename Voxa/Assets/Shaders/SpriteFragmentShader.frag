#version 330 core

uniform sampler2D spriteTexture;

in vec3 fPosition;
in vec4 fColor;
in vec2 fTexCoord;

out vec4 fragColor;

void main()
{
	fragColor = fColor * texture(spriteTexture, fTexCoord);
}
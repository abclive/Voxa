#version 330 core
in vec2 fTexCoord;
out vec4 fragColor;

uniform sampler2D textTexture;
uniform vec3 textColor;

void main()
{
    vec4 sampled = vec4(1.0, 1.0, 1.0, texture(textTexture, fTexCoord).r);
	fragColor = vec4(textColor.x, textColor.y, textColor.z, 1.0) * sampled;
}  
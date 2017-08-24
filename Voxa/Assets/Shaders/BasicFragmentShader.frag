#version 150

uniform sampler2D tex;

struct Light {
	vec3 position;
	vec3 color;
};

uniform Light light;
uniform vec3 ambientLightColor;
uniform float ambientLightStrength;

in vec3 fPosition;
in vec4 fColor; // must match name in vertex shader
in vec2 fTexCoord;
in vec3 fNormal;

out vec4 fragColor; // first out variable is automatically written to the screen

void main()
{
	vec3 normal = normalize(fNormal);
	vec3 lightDir = normalize(light.position - fPosition);
	float diff = max(dot(normal, lightDir), 0.0);
	vec3 diffuseColor = diff * light.color;

	vec4 ambientColor = vec4((ambientLightStrength * ambientLightColor), 1);

	vec4 objectColor = (ambientColor + vec4(diffuseColor, 1)) * fColor;
	fragColor = texture(tex, fTexCoord) * objectColor;
}
#version 330 core

struct Material {
	sampler2D diffuseMap;
	sampler2D specularMap;
	vec3	  ambientColor;
	vec3	  diffuseColor;
	vec3	  specularColor;
	float	  shininess;
};

struct Light {
	vec3 position;
	vec3 color;
};

uniform Material material;
uniform Light light;
uniform vec3 ambientLightColor;
uniform float ambientLightStrength;
uniform vec3 cameraPosition;

in vec3 fPosition;
in vec4 fColor; // must match name in vertex shader
in vec2 fTexCoord;
in vec3 fNormal;

out vec4 fragColor; // first out variable is automatically written to the screen

void main()
{
	// Diffuse lighting calculations
	vec3  normal = normalize(fNormal);
	vec3  lightDir = normalize(light.position - fPosition);
	float diff = max(dot(normal, lightDir), 0.0);
	vec3  diffuse = light.color * (diff * material.diffuseColor) * vec3(texture(material.diffuseMap, fTexCoord));

	// Specular lighting calculations
	vec3  viewDir = normalize(cameraPosition - fPosition);
	vec3  reflectDir = reflect(-lightDir, normal);
	float spec = pow(max(dot(viewDir, reflectDir), 0.0), material.shininess);
	vec3  specular = light.color * (spec * material.specularColor) * vec3(texture(material.specularMap, fTexCoord)); 

	// Ambient lighting calculations
	vec3 ambient = ambientLightStrength * ambientLightColor * material.ambientColor * vec3(texture(material.diffuseMap, fTexCoord));

	fragColor = vec4(ambient + diffuse + specular, 1);
}
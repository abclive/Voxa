#version 330 core

#define MAX_LIGHTS 4

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
uniform Light lights[MAX_LIGHTS];
uniform int lightsCount;

uniform vec3 ambientLightColor;
uniform float ambientLightStrength;
uniform vec3 cameraPosition;

in vec3 fPosition;
in vec4 fColor;
in vec2 fTexCoord;
in vec3 fNormal;

out vec4 fragColor;

vec3 CalcLight(Light cLight, vec3 normal)
{
	vec3  lightDir = normalize(cLight.position - fPosition);
	
	// Diffuse lighting calculations
	float diff = max(dot(normal, lightDir), 0.0);
	vec3  diffuse = cLight.color * (diff * material.diffuseColor) * vec3(texture(material.diffuseMap, fTexCoord));

	// Specular lighting calculations
	vec3  viewDir = normalize(cameraPosition - fPosition);

	// Blinn Phong
	vec3  halfwayDir = normalize(lightDir + viewDir);  
    float spec = pow(max(dot(normal, halfwayDir), 0.0), material.shininess);

	// Classic Phong
	//vec3  reflectDir = reflect(-lightDir, normal);
	//float spec = pow(max(dot(viewDir, reflectDir), 0.0), material.shininess);

	vec3 specular = cLight.color * (spec * material.specularColor) * vec3(texture(material.specularMap, fTexCoord)); 

	// Ambient lighting calculations
	vec3 ambient = vec3(fColor.r, fColor.g, fColor.b) * ambientLightStrength * ambientLightColor * material.ambientColor * vec3(texture(material.diffuseMap, fTexCoord));

	vec3 lightColor = (ambient + diffuse + specular);
	return lightColor;
}

void main()
{
	vec3 normal = normalize(fNormal);
	vec3 lightColor = vec3(0, 0, 0);

	for (int i = 0; i < lightsCount; i++) {
		lightColor += CalcLight(lights[i], normal);
	}

	fragColor = vec4(lightColor, 1);
}


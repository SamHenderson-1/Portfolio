//
// Simple passthrough fragment shader
//

#define PI 3.14159265

varying vec2 v_vTexcoord;
varying vec4 v_vColour;

uniform vec4 uvs;
uniform float offset;

void main()
{
	vec2 adjusted_texcoord = v_vTexcoord;
	
    adjusted_texcoord.x = adjusted_texcoord.x + (uvs.z - uvs.x) / 10. * sin(adjusted_texcoord.y * 2. * PI / (uvs.w - uvs.y) + offset * 2. * PI); 
	adjusted_texcoord.x = clamp(adjusted_texcoord.x, uvs.x, uvs.z);
	
	gl_FragColor = v_vColour * texture2D( gm_BaseTexture, adjusted_texcoord );
}

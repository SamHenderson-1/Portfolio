//
// Simple passthrough fragment shader
//
varying vec2 v_vTexcoord;
varying vec4 v_vColour;

uniform float blur_steps;
uniform vec2 texel_size;
uniform vec4 uvs;
uniform vec2 blur_vector;
uniform bool premultiply;

uniform bool use_depth_map;
uniform sampler2D depth_map;
uniform vec4 dm_uvs;

uniform float base_depth;
uniform float focus_plane_depth;

float sigma;

vec2 uvs_to_dm_uvs(vec2 coord)
{
	float _width = uvs.z - uvs.x;
	float _height = uvs.w - uvs.x;
	float _dm_width = dm_uvs.z - dm_uvs.x;
	float _dm_height = dm_uvs.w - dm_uvs.x;
	
	return vec2(dm_uvs.x + ((coord.x - uvs.x)/_width)*_dm_width, dm_uvs.y + ((coord.y - uvs.y)/_height)*_dm_height);
}

void set_sigma()
{
	float true_depth = base_depth;
	
	if (use_depth_map)
		true_depth *= texture2D(depth_map, uvs_to_dm_uvs(v_vTexcoord)).r;
	
	float blur_intensity = abs(true_depth - focus_plane_depth);
	blur_intensity *= mix(0.1, 1., abs(0.5 - focus_plane_depth));
	sigma = mix(0.001, 0.5, blur_intensity);
}


float weight(float pos)
{
	return exp(-(pos * pos) / (2.0 * sigma * sigma));
}

void main()
{
    highp vec4 blurred_col = texture2D(gm_BaseTexture, v_vTexcoord);
	
	set_sigma();
	
	vec2 sample1, sample2;
	float offset, sample_weight;
	float total_weight = 1.0;
	float kernel = 2.0 * blur_steps + 1.0;

	for (offset = 1.0; offset <= blur_steps; offset++)
	{
		sample_weight = weight(offset/kernel);
		total_weight += 2.0 * sample_weight;

		sample1 = clamp(v_vTexcoord + offset * texel_size * blur_vector, uvs.xy, uvs.zw);
		sample2 = clamp(v_vTexcoord - offset * texel_size * blur_vector, uvs.xy, uvs.zw);
		blurred_col += (texture2D(gm_BaseTexture, sample1) + texture2D(gm_BaseTexture, sample2)) * sample_weight;
	}
	
	vec4 straight = v_vColour * blurred_col / total_weight;
	
	if (premultiply)
		gl_FragColor = vec4(straight.rgb * straight.a, straight.a);
	else
		gl_FragColor = straight;
}

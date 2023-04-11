// Script assets have changed for v2.3.0 see
// https://help.yoyogames.com/hc/en-us/articles/360005277377 for more information

#macro BLUR shdr_optimized_blur_2_pass
#macro BLUR_STEPS 30

function scr_blur_initialize(_depth, _depth_map = -1, _base_color = c_black)
{
	// Get uniforms.
	u_blur_steps = shader_get_uniform(BLUR, "blur_steps");
	u_texel_size = shader_get_uniform(BLUR, "texel_size");
	u_uvs = shader_get_uniform(BLUR, "uvs");
	u_blur_vector = shader_get_uniform(BLUR, "blur_vector");
	u_premultiply = shader_get_uniform(BLUR, "premultiply");
	u_use_depth_map = shader_get_uniform(BLUR, "use_depth_map");
	si_depth_map = shader_get_sampler_index(BLUR, "depth_map");
	u_depth_map_uvs = shader_get_uniform(BLUR, "dm_uvs");
	u_base_depth = shader_get_uniform(BLUR, "base_depth");
	u_focus_plane_depth = shader_get_uniform(BLUR, "focus_plane_depth");

	// Allocate instance variables to reuse surfaces.	
	srf_marg = -1;
	srf_ping = -1;

	b_depth = _depth;
	use_depth_map = _depth_map >= 0;
	if (use_depth_map)
	{
		depth_map = _depth_map;
		var dm_uvs_all = sprite_get_uvs(_depth_map, 0); 
		depth_map_uvs = [dm_uvs_all[0], dm_uvs_all[1], dm_uvs_all[2], dm_uvs_all[3]];
	}
	
	blur_base_color = _base_color;
}

function scr_compare_surface_dimensions(surface, width, height)
{
	return surface_get_height(surface) == height and surface_get_width(surface) == width;
}

function scr_blur_prefix(sprite = -1, margin = 100)
{
	// Get sprite if none is passed.
	if (sprite == -1)
		sprite = sprite_index;

	// Get sprite uvs.
	var uvs_all = sprite_get_uvs(sprite, 0);

	// Calculate surface size.
	var image_w = image_xscale * sprite_get_width(sprite);
	var image_h = image_yscale * sprite_get_height(sprite);

	// Calculate relative offsets of sprite.
	var r_xoffset = sprite_get_xoffset(sprite) / sprite_get_width(sprite);
	var r_yoffset = sprite_get_yoffset(sprite) / sprite_get_height(sprite);

	scr_blur_prefix_dimensions(image_w, image_h, r_xoffset, r_yoffset, margin, (uvs_all[2] - uvs_all[0]), (uvs_all[3] - uvs_all[1]));
}

function scr_blur_prefix_dimensions(image_w, image_h, r_xoffset, r_yoffset, margin, uvsx = 1, uvsy = 1)
{
	var image_w_with_margin = image_w + 2 * margin;
	var image_h_with_margin = image_h + 2 * margin;
	
	// Calculate texel size.
	texel_w = uvsx / image_w;
	texel_h = uvsy / image_h;

	// Initialize surfaces if needed.
	if (!surface_exists(srf_ping))
		srf_ping = surface_create(image_w_with_margin, image_h_with_margin);
	else if (!scr_compare_surface_dimensions(srf_ping, image_w_with_margin, image_h_with_margin))
	{
		surface_free(srf_ping);
		srf_ping = surface_create(image_w_with_margin, image_h_with_margin);
	}

	if (!surface_exists(srf_marg))
		srf_marg = surface_create(image_w_with_margin, image_h_with_margin);
	else if(!scr_compare_surface_dimensions(srf_marg, image_w_with_margin, image_h_with_margin))
	{
		surface_free(srf_marg);
		srf_marg = surface_create(image_w_with_margin, image_h_with_margin);
	}

	// Temporarily store last x and y pos.
	t_x = x;
	t_y = y;

	// Temporarily move for surface printing.
	x = image_w * r_xoffset + margin;
	y = image_h * r_yoffset + margin;

	// Draw onto margin surface for extra area.
	surface_set_target(srf_marg);
	draw_clear_alpha(blur_base_color, 0);
}

function scr_blur_postfix()
{
	// Stop focusing on the srf_marge.
	surface_reset_target();

	// Set up blur shader.
	shader_set(BLUR);
	shader_set_uniform_f(u_blur_steps, BLUR_STEPS);
	shader_set_uniform_f(u_texel_size, texel_w, texel_h);
	shader_set_uniform_f(u_uvs, 0,0,1,1);
	shader_set_uniform_f(u_blur_vector, 0, 1);
	shader_set_uniform_f(u_premultiply, true);
	shader_set_uniform_f(u_base_depth, b_depth);
	shader_set_uniform_f(u_focus_plane_depth, global.focus_plane_depth);
	
	shader_set_uniform_i(u_use_depth_map, use_depth_map);
	if (use_depth_map)
	{
		texture_set_stage(si_depth_map, sprite_get_texture(depth_map, 0));
		shader_set_uniform_f_array(u_depth_map_uvs, depth_map_uvs);
	}
	// Draw onto intermediate surface for vertical blur.
	gpu_set_blendmode_ext(bm_one, bm_inv_src_alpha);
	surface_set_target(srf_ping);
	draw_clear_alpha(blur_base_color, 0);
	draw_surface(srf_marg, 0, 0);
	surface_reset_target();
	
	// Draw onto screen with  horizontal blur.
	shader_set_uniform_f(u_uvs, 0, 0, 1, 1);
	shader_set_uniform_f(u_blur_vector, 1, 0);
	shader_set_uniform_f(u_premultiply, true);

	draw_surface(srf_ping, t_x - x, t_y - y);

	// Clean up draw settings.
	x = t_x;
	y = t_y;

	shader_reset();
	gpu_set_blendmode(bm_normal);
}

function scr_blur_cleanup()
{
	if (surface_exists(srf_ping)) surface_free(srf_ping);
	if (surface_exists(srf_marg)) surface_free(srf_marg);
}
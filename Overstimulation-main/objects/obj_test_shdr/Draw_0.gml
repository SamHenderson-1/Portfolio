/// @description Insert description here
// You can write your code in this editor

#macro TEST_SHADER shdr_drunk_disort

shader_set(TEST_SHADER);
var u_uvs = shader_get_uniform(TEST_SHADER, "uvs");
var u_offset = shader_get_uniform(TEST_SHADER, "offset");

var texture = sprite_get_texture(sprite_index, image_index);
var uvs_all = texture_get_uvs(texture);
var uvs = [uvs_all[0], uvs_all[1], uvs_all[2], uvs_all[3]];

var offset = anim_progress / anim_rate;

shader_set_uniform_f_array(u_uvs, uvs);
shader_set_uniform_f(u_offset, offset);

draw_self();

shader_reset();





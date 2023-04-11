// Script assets have changed for v2.3.0 see
// https://help.yoyogames.com/hc/en-us/articles/360005277377 for more information
function scr_jump(_start_height, _start_scale_h, _start_scale_v, _max_height, _max_scale_factor_h, _max_scale_factor_v, _progress){
	var quad_interpolation = -4*power(_progress-0.5, 2)+1;
	image_xscale = _start_scale_h * lerp(1, _max_scale_factor_h, quad_interpolation);
	image_yscale = _start_scale_v * lerp(1, _max_scale_factor_v, quad_interpolation);
	y = lerp(_start_height, _start_height - _max_height, quad_interpolation);
}
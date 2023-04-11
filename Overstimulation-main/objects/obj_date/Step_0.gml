/// @description Insert description here
// You can write your code in this editor


if (alarm[0] > 0)
{
	scr_jump(start_height, start_scale_h, start_scale_v, jump_height, jump_scale_x, jump_scale_y, alarm[0] / jump_duration);
}
if (!paused) {
	var _dt = delta_time / 1000000;

	blur_amount += blur_rate * _dt;
	blur_amount = min(blur_amount, 1);
}

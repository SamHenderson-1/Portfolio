/// @description Insert description here
// You can write your code in this editor

scr_blur_initialize(0.5);

jumping = false;
start_height = y;
start_scale_h = image_xscale;
start_scale_v = image_yscale;

jump_height = 10;
jump_scale_x = 0.9;
jump_scale_y = 1.1;

jump_duration = 0.25 * room_speed;

blur_amount = 0;
blur_rate = 0.25;

pauseVal = 0;
paused = false;

function start_jump()
{
	alarm[0] = jump_duration;
	jumping = true;
}

function pause() {
	paused = !paused;
	if (paused) {
		pauseVal = alarm[0];
		alarm[0] = -1;
	}
	else
		alarm[0] = pauseVal;
}
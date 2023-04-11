/// @description Insert description here
// You can write your code in this editor

#macro INITIAL_PAUSE_ALARM alarm[0]
#macro FADE_OUT_ALARM alarm[1]
#macro FADE_IN_ALARM alarm[2]
#macro TITLECARD_ALARM alarm[3]

opacity = 0;

visible = true;

pause_duration = 5 * room_speed;
fade_duration = 3 * room_speed;
titlecard_duration = 5 * room_speed;

print_titlecard = false;
pauseVal = 0;
pauseVal1 = 0;
pauseVal2 = 0;
pauseVal3 = 0;
paused = false;

function pause() {
	paused = !paused;
	if (paused) {
		pauseVal = alarm[0];
		alarm[0] = -1;
		pauseVal1 = alarm[1];
		alarm[1] = -1;
		pauseVal2 = alarm[2];
		alarm[2] = -1;
		pauseVal3 = alarm[3];
		alarm[3] = -1;
	}
	else
	{
		alarm[0] = pauseVal;
		alarm[1] = pauseVal1;
		alarm[2] = pauseVal2;
		alarm[3] = pauseVal3;
	}
}

#region Events
pause_end = new DateEvent();
fade_out_end = new DateEvent();
fade_in_end = new DateEvent();
titlecard_end = new DateEvent();
animation_end = new DateEvent();
#endregion

#region Animation Drivers
function pause_animation_driver()
{
	INITIAL_PAUSE_ALARM = pause_duration;
}

function fade_out_animation_driver()
{
	opacity = 0;
	FADE_OUT_ALARM = fade_duration;
}

function fade_in_animation_driver()
{
	opacity = 1;
	print_titlecard = true;
	FADE_IN_ALARM = fade_duration;
}

function titlecard_animation_driver()
{
	opacity = 0;
	TITLECARD_ALARM = titlecard_duration;
}

function end_full_animation()
{
	animation_end.invoke();
}
#endregion

#region Driver Event Assignment
pause_end.add_listener(fade_out_animation_driver, self);
fade_out_end.add_listener(fade_in_animation_driver, self);
fade_in_end.add_listener(titlecard_animation_driver, self);
titlecard_end.add_listener(end_full_animation, self);
#endregion

pause_animation_driver();
/// @description Insert description here
// You can write your code in this editor

// Inherit the parent event
event_inherited();

enum SlideState
{
	IDLE,
	ENTERING,
	EXITING
}


slide_state = SlideState.IDLE;
visible = false;

offscreen_x = -image_xscale * sprite_get_width(sprite_index);
onscreen_x = x;
x = offscreen_x;
slide_duration = 0.75 * room_speed;
blocking = false;

pauseVal1 = 0;
paused = false;

function pause() {
	paused = !paused;
	if (paused) {
		pauseVal1 = alarm[1];
		alarm[1] = -1;
	}
	else
	{
		alarm[1] = pauseVal1;
	}
}

function start_slide_enter()
{
	start_slide(SlideState.ENTERING, false);
}

function start_slide_exit()
{
	start_slide(SlideState.EXITING, false);
}

function start_slide_enter_block()
{
	start_slide(SlideState.ENTERING, true);
}

function start_slide_exit_block()
{
	start_slide(SlideState.EXITING, true);
}

// Assumes _slide_state is ENTERING or EXITING, but never IDLE
function start_slide(_slide_state, _blocking = false)
{
	// Set up alarm
	var from_idle = slide_state == SlideState.IDLE;
	var opposite_state = slide_state != _slide_state;
	if ((! from_idle) && opposite_state)
		alarm[1] = slide_duration - alarm[1];
	else if (from_idle)
		alarm[1] = slide_duration;
	
	
	// Set up other parameters
	slide_state = _slide_state;
	if (slide_state == SlideState.ENTERING)
		visible = true;
	if ((!blocking) && _blocking)
		BLOCK_DIALOGUE
	else if (blocking && !_blocking)
		UNBLOCK_DIALOGUE
	blocking = _blocking;
}

function end_slide()
{
	if (slide_state == SlideState.EXITING)
		visible = false;
	slide_state = SlideState.IDLE;
	if (blocking)
		UNBLOCK_DIALOGUE
	blocking = false;
}





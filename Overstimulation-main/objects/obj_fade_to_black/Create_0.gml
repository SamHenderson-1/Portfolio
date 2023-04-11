/// @description Insert description here
// You can write your code in this editor

#macro FADE_ALARM alarm[0]

opacity = 1;

enum FadeState
{
	IDLE,
	IN,
	OUT
}

visible = true;
fade_state = FadeState.IDLE;
fade_duration = 1 * room_speed;
blocking = false;

paused = false;
pauseVal = 0;

function pause() {
	paused = !paused;
	if (paused) {
		pauseVal = alarm[0];
		alarm[0] = -1;
	}
	else
		alarm[0] = pauseVal;
}

function fade_in()
{
	start_fade(FadeState.IN);
}

function fade_out()
{
	start_fade(FadeState.OUT);
}

function fade_in_block()
{
	start_fade(FadeState.IN, true);
}

function fade_out_block()
{
	start_fade(FadeState.OUT, true);
}

// Assumes _slide_state is IN or OUT, but never IDLE
function start_fade(_fade_state, _blocking = false)
{
	// Set up alarm
	var from_idle = fade_state == FadeState.IDLE;
	var opposite_state = fade_state != _fade_state;
	if ((! from_idle) && opposite_state)
		FADE_ALARM = fade_duration - FADE_ALARM;
	else if (from_idle)
		FADE_ALARM = fade_duration;
	
	
	// Set up other parameters
	fade_state = _fade_state;
	switch(fade_state)
	{
		case FadeState.IN:
			opacity = 1;
			break;
		case FadeState.OUT:
			opacity = 0;
			break;
	}
	visible = true;
	
	if ((!blocking) && _blocking)
		BLOCK_DIALOGUE
	else if (blocking && !_blocking)
		UNBLOCK_DIALOGUE
	blocking = _blocking;
}

function end_fade()
{
	if (fade_state == FadeState.IN)
		visible = false;
	fade_state = FadeState.IDLE;
	if (blocking)
		UNBLOCK_DIALOGUE
	blocking = false;
}

function set_on()
{
	set_fade(1);
}

function set_off()
{
	set_fade(0);
}

function set_fade(_opacity)
{
	fade_state = FadeState.IDLE;
	opacity = _opacity;
	visible = true;
	if (blocking)
		UNBLOCK_DIALOGUE
	blocking = false;
}

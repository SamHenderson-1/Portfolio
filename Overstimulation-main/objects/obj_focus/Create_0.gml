/// @description Insert description here
// You can write your code in this editor

// Rates at which the focus is controlled by the
// focus randomizer and the user in focus ([0,1]) / sec.
#macro DEFOCUS_RATE 0.5
#macro FOCUS_RATE 1.25

// The duration of pauses between sequences of
// focus randomization in seconds.
#macro TIME_BETWEEN_SEQUENCE_MIN 3
#macro TIME_BETWEEN_SEQUENCE_MAX 10

// The duration of sequences of focus randomization
// in seconds.
#macro SEQUENCE_DURATION_MIN 0.5
#macro SEQUENCE_DURATION_MAX 2.5

function focus() {
	var _dt = delta_time / 1000000;
	
	// Adjust focus via player input.
	if mouse_wheel_down()
		global.focus_plane_depth -= _dt*FOCUS_RATE;
	if mouse_wheel_up()
		global.focus_plane_depth += _dt*FOCUS_RATE;
	
	// If we're supposed to be focusing...
	if (active)
	{
		// Check if we're waiting for a sequence to start.
		if (time_to_next_focus_sequence > 0)
			time_to_next_focus_sequence -= _dt; // Decrement the timer until the sequence start.
		// Check if we're in the middle of a sequence.
		else if (focus_sequence_duration > 0)
		{
			// Adjust focus via sequence specifications.
			global.focus_plane_depth += focus_direction * _dt * DEFOCUS_RATE;
			// Decrement the timer for the sequence.
			focus_sequence_duration -= _dt;
		}
		// If we're not waiting for a sequence to start, nor are we
		// in the middle of a sequence, we need to set the parameters
		// for a new sequence.
		else
			generate_focus_sequence_parameters();
	}
	
	
	global.focus_plane_depth = clamp(global.focus_plane_depth, 0, 1);
}
	
function focusTrigger() {
	active = true;
	generate_focus_sequence_parameters();
}

function focusHalt() {
	active = false;
	global.focus_plane_depth = 0.5;
}

function generate_focus_sequence_parameters()
{
	// Focus direction is either -1 or 1
	focus_direction = -1 + 2 * round(random(1));
	time_to_next_focus_sequence = lerp(TIME_BETWEEN_SEQUENCE_MIN, TIME_BETWEEN_SEQUENCE_MAX, random(1));
	focus_sequence_duration = lerp(SEQUENCE_DURATION_MIN, SEQUENCE_DURATION_MAX, random(1));
}

function pause(){
	paused = !paused;
}

active = false;
focus_direction = 1;
time_to_next_focus_sequence = 0;
focus_sequence_duration = 0;
paused = false;
global.focus_plane_depth = 0.5;
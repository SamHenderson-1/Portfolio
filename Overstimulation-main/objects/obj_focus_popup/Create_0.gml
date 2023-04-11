/// @description Insert description here
// You can write your code in this editor

#macro FOCUS_CLOSE_TIP "Scroll up to\nfocus forward!"
#macro FOCUS_FAR_TIP "Scroll down to\nfocus back!"
#macro FOCUS_CLOSE_SPRITE spr_mouse_wheel_up_icon
#macro FOCUS_FAR_SPRITE spr_mouse_wheel_down_icon

#macro DRAW_SPRITE true

#macro TIP_FONT fn_standard
#macro TIP_THRESHOLD 0.2

#macro OFFSET_MAX 5
#macro OFFSET_FREQUENCY 2

paused = false;
is_active = false;
opacity = 0;
tip_string = "";
tip_sprite = FOCUS_CLOSE_SPRITE;

has_shown_close = false;
has_shown_far = false;
has_finished = false;

offset_x = 0;
offset_y = 0;
next_offset_x = 0;
next_offset_y = OFFSET_MAX / 2;
offset_freq_x = 0;
offset_freq_y = floor(1/3*OFFSET_FREQUENCY);

function start_tip()
{
	if (!has_finished)
		is_active = true;
}

function end_tip()
{
	is_active = false;
	if (!has_finished)
		has_finished = has_shown_close && has_shown_far;
}

function pause() {
	paused = !paused;
}

function update_tip()
{
	// Opacity
	opacity = abs(0.5 - global.focus_plane_depth) * 2;
	
	if (opacity < TIP_THRESHOLD)
		opacity = 0;
	else
		opacity = (opacity - TIP_THRESHOLD) / (1 - TIP_THRESHOLD);
	
	// Vibrations
	var offset_intensity = lerp(0, OFFSET_MAX, opacity);
	// X
	if (offset_freq_x == 0)
	{
		next_offset_x = random_range(-offset_intensity, offset_intensity);
		offset_freq_x = OFFSET_FREQUENCY;
	}
	else
	{
		var diff = next_offset_x - offset_x;
		offset_x += diff / offset_freq_x;
		offset_freq_x--;
	}
	// Y
	if (offset_freq_y == 0)
	{
		next_offset_y = random_range(-offset_intensity, offset_intensity);
		offset_freq_y = OFFSET_FREQUENCY;
	}
	else
	{
		var diff = next_offset_y - offset_y;
		offset_y += diff / offset_freq_y;
		offset_freq_y--;
	}

	// Sprites & Text
	if (global.focus_plane_depth < 0.5)
	{
		tip_sprite = FOCUS_CLOSE_SPRITE
		tip_string = FOCUS_CLOSE_TIP;
		has_shown_close = true;
	}
	else if (global.focus_plane_depth > 0.5)
	{
		tip_sprite = FOCUS_FAR_SPRITE
		tip_string = FOCUS_FAR_TIP;
		has_shown_far = true;
	}
	else
		tip_string = "";
}
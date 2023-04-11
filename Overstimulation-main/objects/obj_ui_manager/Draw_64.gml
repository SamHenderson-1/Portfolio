/// @description Draws the response timer.

#macro FILL spr_autism_timer_bar_fill
#macro BAR spr_autism_timer_bar_outline

// If there is a response timer countdown...
if (alarm[1] > 0)
{
	// Draw a bar whose length is proportional to the time left
	// on the response timer.
	draw_set_color(c_white);
	var _full_width = 1024;
	var progress = (alarm[1] / (room_speed * calculate_response_time()));
	var _timed_width = progress * _full_width
	
	var color_full = $50c03f;
	var color_empty = $3f3fc0; 
	
	var color = merge_color(color_empty, color_full, progress);
	
	draw_sprite_ext(FILL, 0, x, y, _timed_width / sprite_get_width(FILL), 64 / sprite_get_height(FILL), 0, color, 1);
	draw_sprite_stretched(BAR, 0, x, y, _full_width, 64);
}


/// @description Insert description here
// You can write your code in this editor

if(!paused) {
	var _w = display_get_gui_width();
	var _h = display_get_gui_height();

	var _color = c_black;

	if (print_titlecard)
	{
		var screen_middle = _w / 2;
		var screen_left_column = _w / 3;
		var screen_right_column = _w * 2 / 3;
	
		var screen_title_height = _h / 3;
		var screen_upper_row_height = screen_title_height + (_h - screen_title_height) / 3;
		var screen_lower_row_height = screen_title_height + (_h - screen_title_height) * 2 / 3;

	
		draw_set_color(c_white);
		draw_set_halign(fa_middle);
		draw_set_valign(fa_center);
	
		draw_sprite(spr_overstimulation_title, 0, screen_middle, screen_title_height);
	
		draw_set_font(fn_standard);
		draw_text(screen_left_column, screen_upper_row_height, "Nevada Black");
		draw_text(screen_right_column, screen_upper_row_height, "William Erignac");
		draw_text(screen_left_column, screen_lower_row_height, "Sam Henderson");
		draw_text(screen_right_column, screen_lower_row_height, "Yanxia Bu");	
	}

	draw_set_halign(fa_left);
	draw_set_valign(fa_top);

	draw_set_color(_color);
	draw_set_alpha(opacity);
	draw_rectangle(0, 0, _w, _h, false);
	draw_set_alpha(1);
}


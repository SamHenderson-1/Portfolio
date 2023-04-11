/// @description Insert description here
// You can write your code in this editor
xx = display_get_gui_width()/2;
yy = 250;
draw_set_font(fn_standard);
draw_set_halign(fa_center);
for (i = 0; i <= y_max; i++) {
	if i = y_pos draw_set_color(c_white);
	else draw_set_color(c_black);
	draw_text(xx, yy+i*150, text[i]);
	}
	
for (i = 0; i <= 2; i++) {
	draw_text(xx*1.5, yy + i*150, control_text[i]);
	}
	
draw_set_halign(fa_left);

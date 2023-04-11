draw_sprite_tiled(spr_menu, sub_image, x, y);
	
xx = display_get_gui_width()/2;
yy = 250;
draw_set_font(fn_you);
draw_set_halign(fa_center);
draw_set_color(c_black);
draw_text(xx, yy+1*150, "Paused");

draw_set_halign(fa_left);


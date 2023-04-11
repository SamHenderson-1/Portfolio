/// @description Insert description here
// You can write your code in this editor


if (is_active)
{
	draw_set_font(TIP_FONT);
	draw_set_alpha(opacity);
	
	if (DRAW_SPRITE)
	{
		draw_sprite(tip_sprite, 0, x + offset_x, y + offset_y);
	}
	else
	{
		var t_halign = draw_get_halign();
		var t_valign = draw_get_valign();
	
		draw_set_halign(fa_middle);
		draw_set_valign(fa_center);

		draw_text(x + offset_x, y + offset_y, tip_string);
	
		draw_set_halign(t_halign);
		draw_set_valign(t_valign);
	}
	draw_set_alpha(1);
	
}
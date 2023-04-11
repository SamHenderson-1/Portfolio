#macro UI_BACKGROUND_SPRITE_BASE_MED spr_textured_nine_slice_no_shadow_med
#macro UI_BACKGROUND_SPRITE_SHADOW_MED spr_shadow_nine_slice_med

#macro UI_BACKGROUND_SPRITE_BASE_SML spr_textured_nine_slice_no_shadow_sml
#macro UI_BACKGROUND_SPRITE_SHADOW_SML spr_shadow_nine_slice_sml


/// @function				scr_draw_ui_background(_x, _y, _width, _height, color, _outline, _outline_color)
/// @description			Draws a rectangle with rounded corners with the 
///							specified parameters.
/// @param {Real} _x		Horizontal position of upper left corner of the box.
/// @param {Real} _y		Vertical position of upper left corner of the box.
/// @param {Real} _width	Horizontal size of the box in pixels.
/// @param {Real} _height	Vertical size of the box in pixels.
/// @param {Color} _color	Color of the background. White by default.
/// @param {Bool} _outline	Whether the rectangle that makes up the background
///							should have a one-pixel outline. Flase by default.
/// @param {Color} _outline_color Color of the outline of the box. Grey by default.
function scr_draw_ui_background(_x, _y, _width, _height){
	
	// Set draw parameters.
	draw_set_halign(fa_left);
	draw_set_valign(fa_top);
	
	draw_sprite_stretched(UI_BACKGROUND_SPRITE_BASE_MED, 0, _x, _y, _width, _height);
	draw_sprite_stretched(UI_BACKGROUND_SPRITE_SHADOW_MED, 0, _x, _y, _width, _height);
}

function scr_draw_ui_background_small(_x, _y, _width, _height){
	// Set draw parameters.
	draw_set_halign(fa_left);
	draw_set_valign(fa_top);
	
	draw_sprite_stretched(UI_BACKGROUND_SPRITE_BASE_SML, 0, _x, _y, _width, _height);
	draw_sprite_stretched(UI_BACKGROUND_SPRITE_SHADOW_SML, 0, _x, _y, _width, _height);
}
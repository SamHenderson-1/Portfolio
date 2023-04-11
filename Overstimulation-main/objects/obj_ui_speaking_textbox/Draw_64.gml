/// @description Draws a background and textbox.
// You can write your code in this editor



// Draw the background.
var full_width = 2 * background_margin + width;
var full_height = 2 * background_margin + height;
background_drawer(x - background_margin, y - background_margin, full_width, full_height);


// Draw the textbox on top.
scr_blur_prefix_dimensions(width, height, 0, 0, background_margin)
event_inherited();
scr_blur_postfix();
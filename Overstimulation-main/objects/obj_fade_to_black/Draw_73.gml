/// @description Insert description here
// You can write your code in this editor

var cam = camera_get_active();

var _w = camera_get_view_width(cam);
var _h = camera_get_view_height(cam);
var _x = camera_get_view_x(cam);
var _y = camera_get_view_border_y(cam);

var _color = c_black;

draw_set_color(_color);
draw_set_alpha(opacity);
draw_rectangle(_x, _y, _x + _w, _y + _h, false);
draw_set_alpha(1);



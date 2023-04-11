/// @description Insert description here
// You can write your code in this editor

last_mouse_disp_x = -1;
last_mouse_disp_y = -1;
expected_mouse_x = -1;
expected_mouse_y = -1;

forces = new Vec2(0,0);
paused = false;

function pause() {
	paused = !paused;
}

function room_to_display_dir(vector)
{
	var rm_w = room_width;
	var rm_h = room_height;
	
	var dis_w = window_get_width();
	var dis_h = window_get_height();
	
	var ratio_x = dis_w / rm_w;
	var ratio_y = dis_h / rm_h;
	
	return new Vec2(vector.x * ratio_x, vector.y * ratio_y);
}

function room_to_display_pos(vector)
{
	var rm_w = room_width;
	var rm_h = room_height;
	
	var dis_w = window_get_width();
	var dis_h = window_get_height();
	
	var ratio_x = dis_w / rm_w;
	var ratio_y = dis_h / rm_h;
	
	return new Vec2(vector.x * ratio_x + window_get_x(), vector.y * ratio_y + window_get_y());
}

function display_to_room_dir(vector)
{
	var rm_w = room_width;
	var rm_h = room_height;
	
	var dis_w = window_get_width();
	var dis_h = window_get_height();
	
	var ratio_x = rm_w / dis_w;
	var ratio_y = rm_h / dis_h;
	
	return new Vec2(vector.x * ratio_x, vector.y * ratio_y);
}

function display_to_room_pos(vector)
{
	var rm_w = room_width;
	var rm_h = room_height;
	
	var dis_w = window_get_width();
	var dis_h = window_get_height();
	
	var ratio_x = rm_w / dis_w;
	var ratio_y = rm_h / dis_h;
	
	return new Vec2((vector.x - window_get_x()) * ratio_x, (vector.y - window_get_y()) * ratio_y);
}

function mouse_distance(_x, _y)
{
	disp_mouse_x = display_mouse_get_x();
	disp_mouse_y = display_mouse_get_y();
	
	var mouse_pos = display_to_room_pos(new Vec2(disp_mouse_x, disp_mouse_y));
	
	return new Vec2(_x - mouse_pos.x, _y - mouse_pos.y);
}

function extrapolate_velocity(current_mouse_x, current_mouse_y, dt)
{
	return display_to_room_dir(new Vec2((current_mouse_x - last_mouse_disp_x)/dt, (current_mouse_y - last_mouse_disp_y)/dt));
}

function extrapolate_velocity_from_mouse_movement(current_mouse_x, current_mouse_y, dt)
{
	var expected_difference = new Vec2(current_mouse_x - expected_mouse_x, current_mouse_y - expected_mouse_y);
	expected_difference.mult_scalar(1 / dt);
	return display_to_room_dir(expected_difference);
}

function add_force(addition)
{
	forces = new Vec2(forces.x + addition.x, forces.y + addition.y);
}

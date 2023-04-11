/// @description Insert description here
// You can write your code in this editor

if(!paused) {
	if (last_mouse_disp_x != -1 && window_has_focus())
	{
		var _dt = delta_time / 1000000;
	
		var newVel = extrapolate_velocity(display_mouse_get_x(), display_mouse_get_y(), _dt);
		var mouseVel = extrapolate_velocity_from_mouse_movement(display_mouse_get_x(), display_mouse_get_y(), _dt);
		
		newVel = new Vec2(newVel.x - mouseVel.x, newVel.y - mouseVel.y);
		
		var mouse_velx = newVel.x;
		var mouse_vely = newVel.y;
	
		mouse_velx += forces.x * _dt;
		mouse_vely += forces.y * _dt;
	
	
		var dx = mouse_velx * _dt;
		var dy = mouse_vely * _dt;
	
		var d_display = room_to_display_dir(new Vec2(dx, dy));
	
		var posx = display_mouse_get_x() + d_display.x;
		var posy = display_mouse_get_y() + d_display.y;
			
		display_mouse_set(posx, posy);
		
		expected_mouse_x = posx;
		expected_mouse_y = posy;
	}
	else
	{
		expected_mouse_x = display_mouse_get_x();
		expected_mouse_y = display_mouse_get_y();
	}
	last_mouse_disp_x = display_mouse_get_x();
	last_mouse_disp_y = display_mouse_get_y();
	
	forces = new Vec2(0,0);
}
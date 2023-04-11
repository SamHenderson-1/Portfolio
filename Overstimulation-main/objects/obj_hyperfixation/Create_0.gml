/// @description Insert description here
// You can write your code in this editor

#macro SPRING_CONSTANT_MIN 0
#macro SPRING_CONSTANT_MAX 300
intensity = 1;

paused = false;
mouse_rbody = instance_find(obj_mouse_rbody, 0);
hyperfixation_target = new Vec2(0,0);

active = false;

function spring_force()
{
	var distance = mouse_rbody.mouse_distance(hyperfixation_target.x,hyperfixation_target.y);
	
	var force = copy_vector(distance);
	force.mult_scalar(lerp(SPRING_CONSTANT_MIN, SPRING_CONSTANT_MAX, intensity));
	
	mouse_rbody.add_force(force);
}

function set_target_button(button)
{
	hyperfixation_target = new Vec2(button.x + button.width / 2, button.y + button.height / 2);
}

function pause(){
	paused = !paused;
}

function set_intensity(_intensity)
{
	intensity = clamp(_intensity, 0, 1);
}

function set_active(_active)
{
	active = _active;
}

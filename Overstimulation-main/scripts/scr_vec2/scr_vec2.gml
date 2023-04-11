// Script assets have changed for v2.3.0 see
// https://help.yoyogames.com/hc/en-us/articles/360005277377 for more information
function Vec2(_x, _y) constructor
{
	x = _x;
	y = _y;
	
	static magnitude = function()
	{
		return sqrt(x * x + y * y);
	};
	
	static normalize = function()
	{
		var mag = magnitude();
		x /= mag;
		y /= mag;
	};
	
	static mult_scalar = function(_scalar)
	{
		x *= _scalar;
		y *= _scalar;
	}
}

function copy_vector(vector)
{
	return new Vec2(vector.x, vector.y);
}
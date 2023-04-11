// Script assets have changed for v2.3.0 see
// https://help.yoyogames.com/hc/en-us/articles/360005277377 for more information
function Signal() constructor
{
	event = new DateEvent();
	blocked = false;
	triggered = false;
	
	static s_block = function()
	{
		blocked = true;
	}
	
	static s_unblock = function()
	{
		blocked = false;
		s_check_trigger();
	}
	
	static s_trigger = function()
	{
		triggered = true;
		s_check_trigger();
	}
	
	static s_check_trigger = function()
	{
		if (triggered and !blocked)
		{
			triggered = false;
			event.invoke();
		}
	}
	
	static s_add_listener = function(_callback, _listener)
	{
		event.add_listener(_callback, _listener);
	}
}
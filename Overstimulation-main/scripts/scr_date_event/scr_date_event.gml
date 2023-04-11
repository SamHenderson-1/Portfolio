// Script assets have changed for v2.3.0 see
// https://help.yoyogames.com/hc/en-us/articles/360005277377 for more information

/// @function				DateEvent()
/// @description			Creates date event. A date event
///							is a struct that keeps track of
///							multiple callbacks to call
///							on the invoke function call.
function DateEvent() constructor
{
	
	listener_count = 0;
	// The function to call on invoke.
	callbacks = [];
	// The scope to use when calling the funciton.
	callback_scopes = [];
	
	/// @function			add_listener(_callback, _listener)
	/// @description		Adds a listener (callback + scope)
	///						that will be notified when this
	///						event is invoked.
	/// @param {Function} _callback The function to call.
	/// @param {Object} _listener The scope to call the function in.
	static add_listener = function(_callback, _listener)
	{
		callbacks[listener_count] = _callback;
		callback_scopes[listener_count] = _listener;
		listener_count++;
	}
	/// @function			invoke()
	/// @description		Calls of the callbacks of this event.
	///						In other words, notifies all listeners
	///						of this event.
	static invoke = function(_parameters = noone)
	{
		for (var i = 0; i < listener_count; i++)
		{
			var _to_call = callbacks[i];
			with (callback_scopes[i])
			{
				_to_call(_parameters);
			}
		}
	}
}
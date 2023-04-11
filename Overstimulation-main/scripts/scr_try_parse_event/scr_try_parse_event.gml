// Script assets have changed for v2.3.0 see
// https://help.yoyogames.com/hc/en-us/articles/360005277377 for more information
/// @function				scr_try_parse_event(_event_str)
/// @description			Parses an event into the event name
///							and parameters.
/// @param {String} _event_str A trimmed string representation of an event.
///							can be in one of two forms (parameterless, and 
///							with parameters):
///							EVENT_NAME
///							EVENT_NAME(PARAMETER, ...)
/// @returns {Array}		An array where the first element is the
///							name of the event and the second is the
///							parameters of the event. The name is
///							not checked against globals (no check
///							to see if the event exists). Parameters
///							are casted to their correct types.
function scr_try_parse_event(_event_str){
	var _parameter_start = "(";
	var _parameter_end = ")";
	// Separate the event name from its parameters.
	var _event_and_parameters = scr_split(_event_str, _parameter_start);
	// There should be 1 - 2 elements for a well-formed event.
	var _element_count = ds_list_size(_event_and_parameters);
	// If there are more than 2 elements, there's an extra set of parentheses.
	if (_element_count > 2)
	{
		ds_list_destroy(_event_and_parameters);
		return noone;
	}
	// If there is one element, the string is in the first form.
	else if (_element_count == 1)
	{
		ds_list_destroy(_event_and_parameters);
		// If there is an ) parenthesis in an event declaration
		// with no parameters, something has gone wrong.
		if (string_pos(_parameter_end, _event_str) != 0)
			return noone;
		return [_event_str,[]];
	}
	else
	{
		var _event = _event_and_parameters[|0];
		var _parameters = _event_and_parameters[|1];
		ds_list_destroy(_event_and_parameters);
		
		var _param_char_count = string_length(_parameters);
		var _last_char = string_char_at(_parameters, _param_char_count);
		// If the parameter section doesn't end with a ), something
		// went wrong.
		if (_last_char != _parameter_end)
			return noone;
		// Remove the ) at the end of the parameters.
		_parameters = string_copy(_parameters, 1, _param_char_count - 1);
		
		// If there is an ) parenthesis in an event declaration
		// with parameters where both outer parentheses were
		// removed, something has gone wrong.
		if (string_pos(_parameter_end, _parameters) != 0)
			return noone;
		
		var _parsed_parameters = scr_parse_parameters(_parameters);
		return [scr_trim(_event), _parsed_parameters];
	}
}
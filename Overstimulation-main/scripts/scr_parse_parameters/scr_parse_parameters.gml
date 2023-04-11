// Script assets have changed for v2.3.0 see
// https://help.yoyogames.com/hc/en-us/articles/360005277377 for more information
/// @function				scr_parse_parameters(_parameter_string, _separator)
/// @description			Parses a string of parameters to an array
///							on in-game values (e.g. str "2.1" -> real 2.1)
/// @param {String} _parameter_string The trimmed string of parameters
///							separated by the _separator.
/// @param {String} _separator The token that splits separate parameters.
/// @returns {Array}		All the parameters into their casted
///							values.
function scr_parse_parameters(_parameter_string, _separator = ","){
	if _parameter_string == ""
		return [];
	
	// Separate parameters.
	var _split_parameters = scr_split(_parameter_string, _separator);
	var _parameter_count = ds_list_size(_split_parameters);
	var _parsed_parameters = [];
	
	for (var i = 0; i < _parameter_count; i++)
	{
		var _parameter = new ParsedValue(scr_trim(_split_parameters[|i]));
		_parsed_parameters[i] = _parameter;
	}
	
	return _parsed_parameters;
}

/// @function				scr_try_get_number_from_string(_to_check)
/// @description			If the input string is in the
///							format of a real, cast it to a real.
///							Otherwise, return noone.
/// @param {String} _to_check The string to check whether it's a
///							real.
/// @returns {Real}			The string as a casted real, noone
///							if it cannot be casted.
function scr_try_get_number_from_string(_to_check)
{
	var _unmodified_string = _to_check;
	var _char_count = string_length(_to_check);
	
	if (_char_count == 0)
		return noone;
	// Check for minus sign.
	if (string_char_at(_to_check, 1) == "-")
	{
		_to_check = string_copy(_to_check, 2, --_char_count);
	}
	
	if (_char_count == 0)
		return noone;
	
	// Separate real into the whole and fraction. (WHOLE.FRACTION)
	var _wholes_and_fraction = scr_split(_to_check, ".");
	var _part_count = ds_list_size(_wholes_and_fraction);
	// If there are more than two sections, then there
	// are more than one periods in the string.
	if (_part_count > 2)
	{
		ds_list_destroy(_wholes_and_fraction);
		return noone;
	}
	// Check the whole portion.
	var _whole_str = _wholes_and_fraction[|0];
	var _is_number = scr_is_digits(_whole_str);
	// Check the fraction if it's there.
	if (_part_count == 2)
	{
		var _fraction_str = _wholes_and_fraction[|1];
		_is_number = _is_number and scr_is_digits(_fraction_str);
	}
	
	ds_list_destroy(_wholes_and_fraction);
	
	if (_is_number)
		return real(_unmodified_string);
	else
		return noone;
}

/// @function				scr_is_digits(_digits_str)
/// @description			Checks if a string only has digits.
/// @param {String} _digits_str String to check whether it's 
///							only made of digits.
/// @returns {Bool}			Whether the string is only made of
///							digits.
function scr_is_digits(_digits_str)
{
	var _char_count = string_length(_digits_str);
	var _check_digits = string_digits(_digits_str);
	
	return _char_count == string_length(_check_digits);
}
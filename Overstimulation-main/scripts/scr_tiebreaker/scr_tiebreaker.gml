// Script assets have changed for v2.3.0 see
// https://help.yoyogames.com/hc/en-us/articles/360005277377 for more information
function scr_tiebreaker(_params)
{
	var max_value = noone;
	var num_of_max = 1;
	
	var _param_count = array_length(_params);
	
	if (_param_count < 1)
		throw("The MAX event requires at least one parameter. Got " +string(_param_count) + ".");
	
	for (var i  = 0; i < _param_count; i++)
	{
		if (max_value == noone or max_value < _params[i])
		{
			max_value = _params[i];
			num_of_max = 1;
		}
		else if(_params[i] == max_value)
			num_of_max++;
	}
	
	scr_global_set_property_value("max_result", max_value);
	scr_global_set_property_value("max_tie", num_of_max > 1);
}
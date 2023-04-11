// Script assets have changed for v2.3.0 see
// https://help.yoyogames.com/hc/en-us/articles/360005277377 for more information
function scr_parse_conditional(){

}

function Conditional(_conditional_str, _start_index = 1) constructor{
	if string_char_at(_conditional_str, _start_index) != "("
		throw("Expected ( at start of conditional at index " + string(_start_index) + ", but got none.");
		
	
	var operators = ds_map_create();
	ds_map_add(operators, "not", conditional_not);
	ds_map_add(operators, "and", conditional_and);
	ds_map_add(operators, "or", conditional_or);
	ds_map_add(operators, "==", conditional_eq);
	ds_map_add(operators, "!=", conditional_neq);
	ds_map_add(operators, ">", conditional_g);
	ds_map_add(operators, "<", conditional_l);
	ds_map_add(operators, ">=", conditional_ge);
	ds_map_add(operators, "<=", conditional_le);
	
	static parse_operand = function(_operand, _operators)
	{
		if _operand == ""
			return;
		else if (operand_count == 0 and _operand == "not") or (operand_count == 1 and operator == noone)
		{
			if not ds_map_exists(_operators, _operand)
				throw("Got conditional with non-existant operator " + _operand + ".");
			
			operator = _operators[?_operand];
		}
		else
		{
			operands[operand_count] = new ParsedValue(_operand);
			operand_count++;
		}
	}


	operator = noone;
	
	var _index = _start_index + 1;
	operands = [];
	operand_count = 0;
	length = 0;
	
	
	var _char = string_char_at(_conditional_str, _index);
	var _concatenation = "";
	
	while (_char != ")")
	{
		if (_char == " ") 
		{
			if (_concatenation != "")
			{
				parse_operand(_concatenation, operators);
				_concatenation = "";
			}
		}
		else if _char == "("
		{
			parse_operand(_concatenation, operators);
			_concatenation = "";
			var conditional = new Conditional(_conditional_str, _index);
			operands[operand_count] = conditional;
			operand_count++;
			_index += conditional.length;
		}
		else
			_concatenation += _char;
		
		_index++;
		
		if _index > string_length(_conditional_str)
			throw("Expected ) at end of contitional string. Got none.");
			
		_char = string_char_at(_conditional_str, _index);
	}
	
	parse_operand(_concatenation, operators);
	
	if operator == conditional_not
	{
		if operand_count != 1
			throw("Expected conditonal not with 1 parameter at index at " +string(_start_index)+ ". Got " + string(operand_count) + ".");
	}
	else if operand_count != 2
			throw("Expected conditonal with 2 parameters at index at " +string(_start_index)+ ". Got " + string(operand_count) + ".");

	length = _index - _start_index;
	ds_map_destroy(operators);
	
	static evaluate = function()
	{
		if (operator == conditional_not)
			return operator(operands[0].evaluate());
		else
			return operator(operands[0].evaluate(), operands[1].evaluate());
	}
}

function ParsedValue(_value_str) constructor{
	
	static _raw_value = "*";
	is_raw = false;
	
	if _value_str == ""
		value = noone;
	else if string_char_at(_value_str, 1) == _raw_value
	{
		value = string_copy(_value_str, 2, string_length(_value_str) - 1);
		is_raw = true;
	}
	else
	{		
		// If the parameter can be interpreted as
		// as a real, cast it to an int.
		var _value_real = scr_try_get_number_from_string(_value_str);
		if (_value_real != noone)
			value = _value_real;
		else
			value = _value_str;
	}
	
	static evaluate = function()
	{
		if typeof(value) != "string" or is_raw or not scr_global_property_exists(value)
			return value;
		else
			return scr_global_get_property_value(value);
	}
}

function conditional_not(a)
{
	return not a;
}

function conditional_and(a,b)
{
	return a and b;
}

function conditional_or(a,b)
{
	return a or b;
}

function conditional_eq(a,b)
{
	return a==b;
}

function conditional_neq(a,b)
{
	return a!=b;
}

function conditional_g(a,b)
{
	return a > b;
}

function conditional_l(a,b)
{
	return a < b;
}

function conditional_ge(a,b)
{
	return a >= b;
}

function conditional_le(a,b)
{
	return a <= b;
}
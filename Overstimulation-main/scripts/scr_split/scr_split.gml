/// @function				scr_split(_to_split, _splitter)
/// @description			Splits the string into a list of strings
///							based on a splitter.
/// @param {String} _to_split The string to split.
/// @param {String} _splitter The pattern that splits _to_split.
/// @returns {List<String>} _to_split split by _splitter.
function scr_split(_to_split, _splitter){
	var _to_split_len = string_length(_to_split);
	var _splitter_len = string_length(_splitter);
	
	var _split_string = ds_list_create();
	
	var _starting_index = 1;
	
	for (var i = 0; _starting_index + i <= _to_split_len - _splitter_len + 1; i++)
	{
		var _sub_str = string_copy(_to_split, _starting_index + i, _splitter_len);
		
		if (_sub_str == _splitter)
		{
			var _split_portion = string_copy(_to_split, _starting_index, i);
			ds_list_add(_split_string, _split_portion);
			
			_starting_index += i + _splitter_len;
			i = -1;
		}
	}
	
	var _split_portion = string_copy(_to_split, _starting_index, _to_split_len - (_starting_index - 1));
	ds_list_add(_split_string, _split_portion);
	
	return _split_string;
}
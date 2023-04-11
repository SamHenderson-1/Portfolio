/// @function				scr_trim(_to_trim)
/// @description			Removes whitespace at the start
///							and end of a string.
/// @param {String} _to_trim The string to trim.
/// @returns {String}		_to_trim without whitespace at 
///							the start and the end of the string.
function scr_trim(_to_trim){
	
	var _len = string_length(_to_trim);
	
	if (_len > 0)
	{
		var _character_index = 1;
		var _character;
		
		do 
		{
			_character = string_char_at(_to_trim, _character_index);
			
			if (_character != " " and _character != "\n" and _character != "\r")
				break;
			else
			{
				_to_trim = string_delete(_to_trim, _character_index, 1);
				_len--;
			}
			
		}
		until (_character_index > _len);
		
		do 
		{
			_character = string_char_at(_to_trim, _len);
			
			if (_character != " " and _character != "\n" and _character != "\r")
				break;
			else
			{
				_to_trim = string_delete(_to_trim, _len, 1);
				_len--;
			}
			
		}
		until (_len == 0);
	
	}
	
	return _to_trim;
}
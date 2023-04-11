/// @function				scr_open_file(_fname)
/// @description			Opens the given file in included files
///							and extracts the relevant information.
/// @param {String} _fname	The name of the file in included files
///							To open.
function scr_open_file(_fname){
	
	var _file_id = file_text_open_read(_fname);
	// Line number is used for debugging.
	var _line_number = 1;
	// Keys: Section Name, Value: DialogueNode
	var _dialogue_node_map = ds_map_create();
	// Object that parses the text line-by-line.
	var _dialogue_parser = new DialogueParser();
	
	// Parse the file line-by-line.
	while (!file_text_eof(_file_id))
	{
		var _line = file_text_readln(_file_id);
		var _dialogue_node = noone;
		
		// Parse line-by-line. If there's an error, show it with information
		// about where in the file it occured.
		try
		{
			_dialogue_node = _dialogue_parser.read_line(_line);
		}
		catch(_exception)
		{
			if (typeof(_exception) != "string")
				_exception = _exception.message;
			throw ( "\n\rRan into exception on line " + string(_line_number) + " of file " + _fname + ".\n\r\"" + _line + "\"\n\r" + _exception);
		}
		
		// If a dialogue_node was finished, register it in the dictionary.
		if _dialogue_node != noone
		{
			var key = _dialogue_node.name;
			if (ds_map_exists(_dialogue_node_map, key))
				throw ("\n\rTwo sections with the same name " + key + ". Second section ends on line " + string(_line_number) + ".");
			ds_map_add(_dialogue_node_map, key, _dialogue_node);
		}
		
		_line_number++;
	}
	
	// Check that there aren't any unreferenced sections.
	check_all_sections_exist(_dialogue_node_map, _fname);
	
	return _dialogue_node_map;
}

/// @function				check_all_sections_exist(_dialogue_node_map, _fname)
/// @description			Checks whether any sections reference non-existent
///							sections. Also checks for a Start node.
/// @param {Map} _dialogue_node_map The map of dialogue nodes for this file.
/// @param {String} _fname	The name of the file that is being opened. Used for 
///							debugging info.
function check_all_sections_exist(_dialogue_node_map, _fname)
{
	// Check for start.
	if not ds_map_exists(_dialogue_node_map, "Start")
		throw ("\n\rNo Start section was found in file " + _fname + ".");
	
	// Loop through the dialogue node map key-by-key (keys are section names).
	for (var _section_name = ds_map_find_first(_dialogue_node_map);
	not is_undefined(_section_name);
	_section_name = ds_map_find_next(_dialogue_node_map, _section_name)) {
		// Get the dialogue node value for the key (keys are section names).
		var _dialogue_node = _dialogue_node_map[? _section_name];
		// Check if the referenced sections exist in the map.
		switch (_dialogue_node.ending)
		{
			case Dialogue_Node_Ending.JUMP:
				for (var i = 0; i < _dialogue_node.jump_count; i++)
				{
					var _next_section = _dialogue_node.jump_sections[i];
					if (not ds_map_exists(_dialogue_node_map, _next_section)) and _next_section != ""
						throw("\n\rSection " + _section_name + " of file " + _fname + " references non-existent section " + _next_section + ".");
				}
				break;
			case Dialogue_Node_Ending.RESPONSES:
				for (var i = 0; i < _dialogue_node.response_count; i++)
				{
					var _next_section = _dialogue_node.response_ids[i];
					if not ds_map_exists(_dialogue_node_map, _next_section)
						throw("\n\rSection " + _section_name + " of file " + _fname + " references non-existent section " + _next_section + ".");
				}
				break;
		}
	}
}

/// @function				DialogueParser()
/// @description			Creates an empty dialogue parser. The
///							dialogue parser keeps track of the state of
///							dialogue whilst being fed lines to parse.
///							DialogueParsers create and return individual
///							dialogue nodes.
function DialogueParser() constructor
{
	// Static Keywords
	static _open_section = "{";
	static _close_section = "}";
	static _responses = "RESPONSES"
	static _response_separator = "->";
	
	// State Information
	active_dialogue_node = noone;
	in_section = false;
	in_responses = false;
	force_jumps = false;
	force_end_section = false;
	
	/// @function			initialize()
	/// @description		Initializes all the state info for this
	///						DialogueParser.
	/// @returns			The flushed dialogue node.
	static initialize = function()
	{
		in_section = false;
		in_responses = false;
		force_jumps = false;
		force_end_section = false;
		
		var _to_return = active_dialogue_node;
		active_dialogue_node = noone;
		return _to_return;
	}
	
	/// @function			read_line(line)
	/// @description		Reads the line and processes it according
	///						to the given state.
	/// @param {String} line The line to read.
	/// @returns {DialogueNode} A complete dialogue node if we reached
	///						the end of a section.
	static read_line = function(line)
	{
		// Get rid of extraneous whitespace.
		line = scr_trim(line);
		
		if line == ""
			return noone;
		
		if not (in_section or in_responses)
			return read_line_out_all(line);
		else if in_section and not in_responses
			return read_line_in_section(line);
		else if in_section and in_responses
			return read_line_in_responses(line);
		else
			throw ("Entered illegal state: in_section is false and in_responses is true");
	}
	
	/// @function			read_line_out_all(line)
	/// @description		Looks for the start of a new section.
	/// @param {String} line The line to read.
	static read_line_out_all = function(line)
	{
		if line == _open_section
			in_section = true;
		else if line == _close_section
			throw ("Unexpected " + _close_section + " before the start of a section.");
		return noone;
	}
	
	/// @function			read_line_in_section(line)
	/// @description		Reads the line as if it was in the middle
	///						of a section.
	/// @param {String} line The line to read.
	/// @returns {DialogueNode} The completed dialogue node
	///						if we reached the end of a section.
	static read_line_in_section = function(line)
	{
		// If we reach the end of the section, the node is complete.
		if line == _close_section
			return initialize();
		// If we didn't end the section and should have, something has
		// gone wrong.
		else if force_end_section
			throw ("Expected section to end on line " + line + " but didn't.");
		// If we reach the start of a new section in the middle of a
		// section, something has gone wrong.
		else if line == _open_section
			throw ("Unexpected " + _open_section + " in the middle of a section.")
		// If we haven't created a dialogue node, we're expecting a title
		// for our node.
		else if active_dialogue_node == noone
		{
			if line == _responses
				throw ("Unexpected beginning of response contents " + _responses + " in untitled section.")
			else
				active_dialogue_node = new DialogueNode(line);
		}
		// If we have created a dialogue node, this is the contents of the
		// node or responses.
		else
		{
			if line == _responses
				in_responses = true;
			else
			{
				// Check for jump to section. (There should always be one separator with nothing behind it).
				var _split_line = scr_split(line, _response_separator);
				var _number_of_splits = ds_list_size(_split_line);
				
				if (_number_of_splits > 2)
					throw ("Too many separators found in jump " + line + ". Expected 1 separator, found " + string(_number_of_splits - 1) + ".");
				
				if (_number_of_splits == 2)
				{
					var _conditional = scr_trim(_split_line[|0]);
					if (_conditional != "")
					{
						var _conditional_struct = new Conditional(_conditional);
						if (_conditional_struct.length != (string_length(_conditional) - 1))
							throw("Full Conditional was not used for line " + line + ". Conditional was only " + string(_conditional_struct.length + 1) + " characters long, but expected a " + string(string_length(_conditional)) + " long conditional.");
						
						active_dialogue_node.add_jump(scr_trim(_split_line[|1]), _conditional_struct);
						force_jumps = true;
					}
					else
					{
						active_dialogue_node.add_jump(scr_trim(_split_line[|1]));
						force_end_section = true;
					}
				}
				else if force_jumps
						throw ("Expected jump in place of " + line + " but didn't get jump.");
				else
					active_dialogue_node.add_page(line);
			}
		}
		return noone;
	}
	
	/// @function			read_line_in_responses(line)
	/// @description		Reads the line as if it was in the middle
	///						of a responses subsection.
	/// @param {String} line The line to read.
	/// @returns {DialogueNode} The completed dialogue node
	///						if we reached the end of a section. 
	static read_line_in_responses = function(line)
	{
		// If we reached the end of a section, reset all
		// state info and return the dialogue node.
		if line == _close_section
			return initialize();
		// If we reached the start of a new section, that doesn't make
		// any sense.
		else if line == _open_section
			throw ("Unexpected " + _open_section + " in the middle of a section's responses.");
		// If we reached the start of a response subsection, that doesn't
		// make any sense.
		else if line == _responses
			throw ("Unexpected " + _responses + " in a section's responses.");
		// Otherwise, we can split to see what the response looks like
		// and see where it's going to.
		else
		{
			var _split_line = scr_split(line, _response_separator);
			// Response format checking. (There should always be one separator).
			var _number_of_splits = ds_list_size(_split_line);
			if (_number_of_splits < 2)
				throw ("No separator found in response " + line + ". Expected 1 separator.");
			else if (_number_of_splits > 2)
				throw ("Too many separators found in response " + line + ". Expected 1 separator, found " + string(_number_of_splits - 1) + ".");
			// Correct format, adding response to dialogue node.
			active_dialogue_node.add_response(scr_trim(_split_line[|0]), scr_trim(_split_line[|1]));
			// No memory leaks.
			ds_list_destroy(_split_line);
		}
		return noone;
	}
}
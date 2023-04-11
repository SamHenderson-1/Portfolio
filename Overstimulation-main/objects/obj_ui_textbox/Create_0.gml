/// @description Initializes the textbox.

// The kinds of ways the textbox can deal with
// text that extends beyond its bounds.
enum Overflow_Mode {OVERFLOW, WRAP}

#region Public Methods

/// @function				show_text(_text_to_display, _unhidden_text)
/// @description			Shows the provided text. show_text is undefined
///							for text with newlines.
/// @param {String} _text_to_display The text to display in this textbox.
/// @param {String} [_unhidden_text] The full text that will eventually be shown
///							if _text_to_display isn't the full text. Used for
///							wrapping. The characters from 1 to
///							str_len(_text_to_display) much match _text_to_display.
/// @self obj_ui_textbox
function show_text(_text_to_display, _unhidden_text = _text_to_display)
{
	// Typecheck.
	if (typeof(_text_to_display) != "string")
		throw ("Called show_text with a non-string parameter " + string(_text_to_display) + ".");
	else
	{
		// Set appropriate fields.
		to_display = _text_to_display;
		unhidden_text = _unhidden_text;
		str_len = string_length(to_display);
		// Parse text to fit in the box.
		parse_text();
	}
}

/// @function				resize(_width, _height)
/// @descrption				Resizes the text box from the upper lefthand corner.
/// @param {Real} _width	The size of the text box in pixels on the x axis.
/// @param {Real} _height	The size of the text box in pixels on the y axis.
/// @self obj_ui_textbox
function resize(_width, _height)
{
	// Typecheck.
	if (typeof(_width) != "number")
		throw ("Called resize with a non-number parameter for _width " + string(_width) + ".");
	else if (typeof(_height) != "number")
		throw ("Called resize with a non-number parameter for _height " + string(_height) + ".");
	else
	{
		// Set appropriate fields.
		width = _width;
		height = _height;
		// Parse text to fit in the box.
		parse_text();

	}
}

/// @function				set_overflow_modes(_h_overflow, _v_overflow)
/// @descrption				Sets how the text box deals with text that goes 
///							beyond the bounds of the box.
/// @param {Overflow_Mode} _h_overflow How the textbox deals with too much
///							text horizontally. If set to OVERFLOW, _v_overflow
///							has little effect.
/// @param {Overflow_Mode} _v_overflow How the textbox deals with too much
///							text vertically. WRAP actually truncates the top
///							of the text to display.
/// @self obj_ui_textbox
function set_overflow_modes(_h_overflow, _v_overflow)
{
	// Set appropriate fields.
	h_overflow_mode = _h_overflow;
	v_overflow_mode = _v_overflow;
	// Parse text to fit in the box.
	parse_text();
}

/// @function				set_font(_font)
/// @description			Sets the font of the text to display.
/// @param {Font} _font		The font to display text in. By default set to
///							fn_standard font.
/// @self obj_ui_textbox
function set_font(_font = fn_standard)
{
	// Set appropriate fields.
	font = _font;
	// Parse text to fit in the box (fonts have different sizes).
	parse_text();
}

/// @function				set_color(_color)
/// @description			Sets the color the text is displayed in.
/// @param {Color} _color	The color the text should have. By default
///							set to black.
/// @self obj_ui_textbox
function set_color(_color = c_black)
{
	color = _color;
}

/// @function				set_alignments(_h_alignment, _v_alignment)
/// @description			Sets the alignment of the text inside the
///							the box. If set to middle, the text will be
///							centered on the middle of the box, not the
///							upper left corner.
/// @param {Alignment} _h_alignment The horizontal alignment of the text.
///							By default set to fa_left.
///	@param {Alignment} _v_alignment The vertical alignment of the text.
///							By default set to fa_top.
/// @self obj_ui_textbox
function set_alignments(_h_alignment = fa_left, _v_alignment = fa_top)
{
	h_alignment = _h_alignment;
	v_alignment = _v_alignment;
}

#endregion

#region Private Methods

/// @function				parse_text()
/// @description			Parses text to be split into lines to 
///							fit the dimensions and wrapping specs of the
///							text box.
/// @self obj_ui_textbox
function parse_text()
{
	// Set font for size calculations.
	draw_set_font(font);
	// Lines are separated as elements in this
	// list.
	lines = ds_list_create();
	
	// Horizontal overflow handling.
	if (h_overflow_mode == Overflow_Mode.OVERFLOW)
		ds_list_add(lines, to_display)
	else
		parse_horizontal_wrapping(lines, to_display, unhidden_text, str_len, width);
	
	// Vertical overflow handling.
	if (v_overflow_mode == Overflow_Mode.WRAP)
		parse_vertical_wrapping(lines, height);
}

/// @function				parse_horizontal_wrapping(_lines, _to_display, _unhidden_text, _str_len, _width)
/// @description			Handles separating text into multiple lines 
///							such that no line in the text is wider than 
///							the provided width.
/// @param {List<String>} _lines The list to fill with individual lines
///							of text. Expected to be empty.
/// @param {String} _to_display Text to display.
/// @param {String} _unhidden_text The full text of _to_display if any
///							of it is hidden. See show_text method.
/// @param {int64} _str_len	The length of _to_display.
/// @param {Real} _width	The horizontal size of the textbox in pixels.
function parse_horizontal_wrapping(_lines, _to_display, _unhidden_text, _str_len, _width)
{
	// Loop through the text looking for where to separate by line.
	// The start of the current potential line is _start_index while
	// how far we've looked down the potential line is _current_length.
	var _start_index = 1;
	var _current_length = 0;
	
	// While there are characters in _to_display, keep looking through characters.
	while (_str_len - (_start_index - 1 + _current_length) > 0)
	{
		// If there is whitespace at the start of this line, trim it off.
		while(_start_index <= _str_len && (string_char_at(_to_display, _start_index) == " "))
		{
			_start_index++;
		}
		// Keep going through the text while the current line is shorter than the width of the textbox.
		while(_str_len - (_start_index - 1 + _current_length) > 0 && string_width(string_copy(_to_display, _start_index, _current_length)) < _width)
		{
			_current_length += 1;
		}
		// If we stopped the last loop because we were wider than the width of the textbox,
		// we're one character too long. Could be the source of an infinite loop if
		// the box isn't wide enough for a single character.
		var breached_width = string_width(string_copy(_to_display, _start_index, _current_length)) >= _width;
		if (breached_width)
		{
			_current_length--;
		}
		// Prevent a line from ending in the middle of a word by stopping the line before
		// the last word starts.
		if (word_will_over_flow(_unhidden_text, _start_index, _current_length, _width))
		{
			// Keep looking for the start of the last word.
			var _previous_current_length = _current_length;
			while(string_char_at(_to_display, _start_index + _current_length) != " " && _current_length > 0)
			{
				_current_length--;
			}
			// If the entire line has no witespace, we're forced to stop the line in the
			// middle of the word.
			if (_current_length == 0)
				_current_length = _previous_current_length;
		}
		
		// Add the current line to the list of lines and look for the next line.
		ds_list_add(_lines, string_copy(_to_display, _start_index, _current_length))
		_start_index = _start_index + _current_length;
		_current_length = 0;
	}
}

/// @function				parse_vertical_wrapping(_lines, _height)
/// @description			Culls upper lines until all of _lines fits in
///							the textbox height.
/// @param {List<String>} _lines The text to print separated by line.
/// @param {Real} _height	The size of the textbox in pixels on the y axis.
function parse_vertical_wrapping(_lines, _height)
{
	// Concatinate lines until they're longer than the height of the
	// textbox.
	var _sum_string = "";
	// _remove_further is false whilst we haven't breached the height
	// of the textbox.
	var _remove_further = false;
	// Start from the last line and go up.
	for (var i = ds_list_size(_lines) - 1; i >= 0; i--)
	{
		// If we haven't yet breached the height, see if the
		// next line does.
		if (! _remove_further)
		{
			// Line concatination.
			var _next_line = _lines[| i];
			var _next_sum_str = _next_line + "\n" + _sum_string;
			var _next_sum_height = string_height(_next_sum_str);
			// If we haven't breached the height, keep checking.
			if (_next_sum_height < _height)
			{
				_sum_string = _next_sum_str;
			}
			// Otherwise, we can cull this line and the preceeding lines.
			else
			{
				ds_list_delete(_lines, i);
				_remove_further = true;
			}
		}
		// Otherwise, just remove upper lines.
		else
			ds_list_delete(_lines, i);
	}
}

/// @function				word_will_over_flow(full_display_text, starting_index, count, text_width)
/// @description			Determines whether a word at the end of a 
///							line will overflow out of the textbox.
/// @param {String} _full_display_text The full (unhidden) text that will be displayed.
/// @param {int64} _starting_index The starting index of the line to look at.
/// @param {int64} _count	The character count of the line to look at.
/// @param {Real} _text_width The width of the text box in pixels.
/// @return {Bool}			Whether the line will eventually breach the
///							textbox width.
function word_will_over_flow(_full_display_text, _starting_index, _count, _text_width)
{
	// Loop character by character past the line until
	// the line is longer than the width of the textbox,
	// we've reached the end of _full_display_text,
	// or we've reached the end of the last word in the line.
	var _length = string_length(_full_display_text);	
	for(var i = _count; (_starting_index + i) < _length + 2; i++)
	{
		var _extended_line = string_copy(_full_display_text, _starting_index, i);
		var _extended_width = string_width(_extended_line); 
		// Did we reach the end of the word?
		if (string_char_at(_full_display_text, _starting_index + i - 1) == " ")
			return false;
		// Have we breached the width of the textbox?
		else if (_extended_width >= _text_width)
			return true;
	}
	// We reached the end of the full_display_text before
	// breaching the width of the textbox.
	return false;
}

#endregion

#region Initialization

// Size of the textbox.
width = 1024;
height = 160;
// Overflow modes.
h_overflow_mode = Overflow_Mode.WRAP;
v_overflow_mode = Overflow_Mode.WRAP;
// Information about text to display.
to_display = "";
// unhidden_text is the actual full text that will be displayed. 
// Usually the same as to_display, but is the full message
// for an animated text box.
unhidden_text = "";
// Length of to_display.
str_len = 0;
// Text visuals.
font = fn_standard;
color = c_white;
// Alignment.
h_alignment = fa_left;
v_alignment = fa_top;

#endregion
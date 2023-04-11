/// @description Initializes the Speaker Textbox

#region Public Methods

/// @function				write_text(_text_to_write)
/// @description			Writes text a character at a time based
///							on the write speed.
/// @param {String} _text_to_write The full text to start writing out.
/// @self obj_ui_speaking_text_box
function write_text(_text_to_write, skip = 0)
{	
	if (typeof(_text_to_write) != "string")
		throw ("Called show_text with a non-string parameter " + string(_text_to_write) + ".");
	else
	{
		to_write = _text_to_write;
		to_write_len = string_length(_text_to_write);
		progress = clamp(skip, 0, to_write_len);
		finished = false;
	}
}

/// @function				set_write_rate(_write_rate)
/// @description			Sets the speed at which the speaker writes
///							characters in characters per second.
/// @param {Real} _write_rate The speed at which the peaker writes
///							in characters per second.
/// @self obj_ui_speaking_textbox
function set_write_rate(_write_rate)
{
	write_rate = _write_rate;
}

/// @function				set_finish_callback(_object_to_call_back, _function_to_call)
/// @description			Sets the callback for when text has finished writing.
/// @param {Object} _object_to_call_back The context for the callback (who to call
///							_function_to_call on).
/// @param {Function} _function_to_call The function to call when the text has finished
///							writing. This should have no return value nor take any parameters.
/// @self obj_ui_speaking_textbox
function set_finish_callback(_object_to_call_back, _function_to_call)
{
	finish_signal.s_add_listener(_function_to_call, _object_to_call_back);
}

/// @function				set_letter_callback(_object_to_call_back, _function_to_call)
/// @description			Sets the callback for when a character is written.
/// @param {Object} _object_to_call_back The context for the callback (who to call
///							_function_to_call on).
/// @param {Function} _function_to_call The function to call when a character is written.
///							This should have no return value nor take any parameters.
/// @self obj_ui_speaking_textbox
function set_letter_callback(_object_to_call_back, _function_to_call)
{
	callback_object_on_letter = _object_to_call_back;
	on_letter_written = _function_to_call;
}

function block()
{
	blocked = true;
	finish_signal.s_block();
}

function unblock()
{
	blocked = false;
	finish_signal.s_unblock();
}

function pause() {
	paused = !paused;
}
function set_background(_background_drawer)
{
	background_drawer = _background_drawer;
}

#endregion

#region Initialization

// Initialize the textbox.
event_inherited();

// Initialize Blur.
scr_blur_initialize(0.5, -1, c_white);

// to_write is the full text to write, as opposed to 
// inherited to_display, which is the text shown.
to_write = "";
to_write_len = 0;
// progress is how many characters have been written.
progress = 0;
// write_rate is the speed at which characters are
// printed in characters per second.
write_rate = 20;

// background_margin is the margin for drawing the 
// background around the textbox.
background_margin = 30;
background_drawer = scr_draw_ui_background;

// Callbacks for writing characters.
callback_object_on_letter = self;
on_letter_written = function () {};

finished = false;
finish_signal = new Signal();
paused = false;

blocked = false;

write_text("As I walk through the valley where I harvest my grain, I take a look at my wife and realize she's very plain; but that's just perfect for an Amish like me. You know, I shun fancy things like electricity. "+
"At 4:30 in the morning I'm milkin' cows. " +
"Jebediah feeds the chickens and Jacob plows... fool. " +
"And I've been milkin' and plowin' so long that " +
"even Ezekiel thinks that my mind is gone. " +
"I'm a man of the land, I'm into discipline. " +
"Got a Bible in my hand and a beard on my chin. " +
"But if I finish all of my chores and you finish thine, " +
"then tonight we're gonna party like it's 1699. " +
"We been spending most our lives " +
"living in an Amish paradise. " +
"I've churned butter once or twice" +
" living in an Amish paradise. " +
"It's hard work and sacrifice" +
" living in an Amish paradise." +
"We sell quilts at discount price" +
" living in an Amish paradise. PachycephalosaurusPachycephalosaurusPachycephalosaurusPachycephalosaurusPachycephalosaurus");

set_alignments(fa_left, fa_top);

#endregion
/// @description Initializes the Text Button.


#region Public Methods

// @function				set_callbacks(_callback_object, callback_id, _on_press, _on_down, _on_up)
// @description				Sets the callbacks for all the types of actions the button
//							can experience.
// @param {Object} _callback_object The context for which to call callbacks on.
// @param _callback_id An identifying parameter passed to all callbacks.
// @param {Function} _on_press The function to call whilst the button is being
//							held down. Should take the identifying parameter.
// @param {Function} _on_down The function to call the frame the button starts
//							being pressed. Should take the identifying parameter.
// @param {Function} _on_up The function to call the frame the button stops
//							being pressed. Should take the identifying parameter.
// @self obj_ui_textbutton
function set_callbacks(_callback_object, _callback_id, _on_press, _on_down, _on_up)
{
	callback_object = _callback_object;
	callback_id = _callback_id;
	on_press = _on_press;
	on_down = _on_down;
	on_up = _on_up;
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

// Initialize the textbox for the button.
event_inherited();

// Initialize callbacks.
callback_object = self;
callback_id = noone;
on_press = function () {};
on_down = function () {};
on_up = function () {};
paused = false;

// background_margin is the extra size of the
// background drawn behind the textbox.
background_margin = 15;
background_drawer = scr_draw_ui_background_small;

set_alignments(fa_middle, fa_middle);

show_text("This is a button.");

resize(240, 100);

#endregion
/// @description Insert description here
// You can write your code in this editor
display_set_gui_size(1366,768);

#macro EXTRA_HEIGHT 300

function create_buttons() { 
	x_pos = (room_width - 350) / 2;
	for(var i = 0; i < array_length(text); i++)
		{
			y_pos = (i + 1) * ((room_height - EXTRA_HEIGHT) - (50 * array_length(text))) / (array_length(text) + 1) + i * 50 + EXTRA_HEIGHT;
			// Create button instance (position in screen is determined by manager's position).
			var _button = instance_create_layer(x_pos, y_pos, layer_get_id("MenuButtons"), obj_ui_textbutton);
			// Set button specs.
			_button.set_callbacks(self, i, _button.on_press, scr_menu, _button.on_up);
			_button.show_text(text[i]);
			_button.set_overflow_modes(Overflow_Mode.WRAP, Overflow_Mode.OVERFLOW);
			_button.resize(350, 50);
		}
}


text[0] = "Start";
text[1] = "Credits";
text[2] = "Exit Game";
control_text[0] = "Press ESC. to Pause";
control_text[1] = "Scroll up or down to focus";
control_text[2] = "Click responses to choose them";
sub_image = 0;
create_buttons();
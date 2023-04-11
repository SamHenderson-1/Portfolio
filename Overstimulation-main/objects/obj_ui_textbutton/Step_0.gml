/// @description Handles button input.

// Initially dreived from https://www.youtube.com/watch?v=jsWPUuwB1RQ

if(!paused) {
	// Check if the mouse is on the button.
	var _mouseX = device_mouse_x_to_gui(0);
	var _mouseY = device_mouse_y_to_gui(0);
	var _mouse_on_button = point_in_rectangle(_mouseX, _mouseY, x - background_margin, y - background_margin, x + width + 2*background_margin, y + height + 2*background_margin);

	// If the mouse is on the button and...
	if (_mouse_on_button)
	{
		// Dummy callback.
		var _to_call = function (_) {};
	
		// ...the mouse is being pressed for the first frame...
		if (mouse_check_button_pressed(mb_left))
		{
			var _to_call = on_down;
		}
		// ...the mouse is being held...
		else if (mouse_check_button(mb_left))
		{
			var _to_call = on_press;
		}
		// ...the mouse is being released this frame...
		else if (mouse_check_button_released(mb_left))
		{
			var _to_call = on_up;
		}
	
		// ...invoke the appropriate callback.
		var _callback_id = callback_id;
		with (callback_object)
		{
			_to_call(_callback_id);
		}
	}
}
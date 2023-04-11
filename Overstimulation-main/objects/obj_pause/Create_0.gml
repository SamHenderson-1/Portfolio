function pause_game() {
	with(all) {
		event_user(0);
	}
	pauseButtons = !pauseButtons;
	self.visible = !self.visible;
	if(pauseButtons) {
		_button = instance_create_layer(50, 50, layer_get_id("PauseButton"), obj_ui_textbutton);
		// Set button specs.
		_button.set_callbacks(self, 3, _button.on_press, scr_menu, _button.on_up);
		_button.show_text("Return to Start Menu");
		_button.set_overflow_modes(Overflow_Mode.WRAP, Overflow_Mode.OVERFLOW);
		_button.resize(500, 50);
	}
	else
		instance_destroy(_button);
		
}
pauseButtons = false;
_button = -1;
sub_image = 0;


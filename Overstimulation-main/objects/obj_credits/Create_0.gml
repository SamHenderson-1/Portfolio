/// @description Insert description here
// You can write your code in this editor
display_set_gui_size(1366,768);

text[0] = "Producer, Writing and Sound Design - Nevada Black";
text[1] = "Lead Programmer - William Erignac";
text[2] = "Programming and Design - Sam Henderson";
text[3] = "Art - Yanxia Bu";
text[4] = "<-";
y_max = array_length_1d(text)-1;
var _button = instance_create_layer(50, 50, layer_get_id("BackButton"), obj_ui_textbutton);
// Set button specs.
_button.set_callbacks(self, 3, _button.on_press, scr_menu, _button.on_up);
_button.show_text(text[4]);
_button.set_overflow_modes(Overflow_Mode.WRAP, Overflow_Mode.OVERFLOW);
_button.resize(50, 50);
sub_image = 0;

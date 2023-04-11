/// @description Insert description here
// You can write your code in this editor


scr_global_get_event("FADE_IN").add_listener(fade_in, self);
scr_global_get_event("FADE_OUT").add_listener(fade_out, self);
scr_global_get_event("FADE_FILL").add_listener(set_on, self);
scr_global_get_event("FADE_CLEAR").add_listener(set_off, self);
scr_global_get_event("FADE_IN_BLOCK").add_listener(fade_in_block, self);
scr_global_get_event("FADE_OUT_BLOCK").add_listener(fade_out_block, self);


/// @description Insert description here
// You can write your code in this editor

scr_global_get_event("FOCUS").add_listener(focusTrigger, self);
scr_global_get_event("END_FOCUS").add_listener(focusHalt, self);

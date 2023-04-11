/// @description Insert description here
// You can write your code in this editor


scr_global_get_event("WAITRESS_JUMP").add_listener(start_jump, self);
scr_global_get_event("WAITRESS_ENTER").add_listener(start_slide_enter, self);
scr_global_get_event("WAITRESS_EXIT").add_listener(start_slide_exit, self);
scr_global_get_event("WAITRESS_ENTER_BLOCK").add_listener(start_slide_enter_block, self);
scr_global_get_event("WAITRESS_EXIT_BLOCK").add_listener(start_slide_exit_block, self);

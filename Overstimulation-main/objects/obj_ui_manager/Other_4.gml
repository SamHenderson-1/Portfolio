/// @description Insert description here
// You can write your code in this editor


// Get first dialogue after it has been loaded
// (if done in Create, there is no guarantee that
// the dialogue would have been loaded by the time
// this is called).
retrieve_dialogue("Start");

scr_global_get_event("INCREMENT_SPEED").add_listener(increment_pacing_speed, self);
scr_global_get_event("DECREMENT_SPEED").add_listener(decrement_pacing_speed, self);
scr_global_get_event("SET_SPEED").add_listener(set_pacing_speed, self);
scr_global_get_event("SAVE_SPEED").add_listener(save_pacing_speed, self);
scr_global_get_event("LOAD_SPEED").add_listener(load_pacing_speed, self);
scr_global_get_event("SOUND_BLOCK").add_listener(play_blocking_sound, self);
scr_global_get_event("SKIP_PAUSE").add_listener(set_skip_pause, self);
scr_global_get_event("HYPERFIX").add_listener(set_hyperfixation_response, self);



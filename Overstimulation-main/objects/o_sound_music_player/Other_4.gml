/// @description Insert description here
// You can write your code in this editor


scr_global_get_event("MUSIC_START").add_listener(start_music, self);
scr_global_get_event("MUSIC_STOP").add_listener(end_music, self);


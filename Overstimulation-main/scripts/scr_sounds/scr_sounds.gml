// Script assets have changed for v2.3.0 see
// https://help.yoyogames.com/hc/en-us/articles/360005277377 for more information
#macro SOUNDS_MAPPING global.sound_effects 

function scr_create_sound_mapping()
{
	SOUNDS_MAPPING = ds_map_create();
	ds_map_add(SOUNDS_MAPPING, "b_knock", sfx_bathroom_knock);
	ds_map_add(SOUNDS_MAPPING, "b_slam", sfx_bathroom_slam);
	ds_map_add(SOUNDS_MAPPING, "confused", sfx_expression_confused);
	ds_map_add(SOUNDS_MAPPING, "shocked", sfx_expression_shocked);
	ds_map_add(SOUNDS_MAPPING, "phone", sfx_phone_response);
}

function scr_destroy_sound_mapping()
{
	ds_map_destroy(SOUNDS_MAPPING);
}

function scr_get_sound(effect_name)
{
	return SOUNDS_MAPPING[? effect_name];
}

function scr_check_sound_exists(effect_name)
{
	return ds_map_exists(SOUNDS_MAPPING, effect_name);
}


function scr_play_sound(args){
	
	if (array_length(args) < 1)
		throw("Playing a sound requires at least one parameter, the name of the sound to play");
		
	var sound_name = args[0];
	
	if (!scr_check_sound_exists(sound_name))
		throw("Sound " + sound_name + " could not be found. Make sure the name wasn't mispelled.");
	
	return audio_play_sound(scr_get_sound(sound_name), 50, false);
}
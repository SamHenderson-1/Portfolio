// Script assets have changed for v2.3.0 see
// https://help.yoyogames.com/hc/en-us/articles/360005277377 for more information

#macro WAITRESS_FONT_FILEPATH "extra_fonts/single-day-regular.ttf"

#macro CHARPROF_MAPPING global.character_profiles

function CharacterProfile(_name, _font = fn_standard, _speech_audio = sfx_narrator_voice) constructor
{
	name = _name;
	font = _font;
	speech_audio = _speech_audio;
}

function scr_create_character_profiles()
{
	CHARPROF_MAPPING = ds_map_create();
	
	fn_waitress = font_add(WAITRESS_FONT_FILEPATH, 23, false, false, 32, 128);
	
	if (fn_waitress == -1)
		show_debug_message("FONT ERROR: Could not find font " + WAITRESS_FONT_FILEPATH + " in included files.");
	
	ds_map_add(CHARPROF_MAPPING, "Him" , new CharacterProfile("Him", fn_date, sfx_dialogue_voice));
	ds_map_add(CHARPROF_MAPPING, "Waitress" , new CharacterProfile("Waitress", fn_waitress, sfx_dialogue_voice_waitress));
	ds_map_add(CHARPROF_MAPPING, "You" , new CharacterProfile("You", fn_you, sfx_dialogue_voice_placeholder));
}

function scr_get_character_profile(_name)
{
	if (ds_map_exists(CHARPROF_MAPPING, _name))
		return CHARPROF_MAPPING[?_name];
	else
		return new CharacterProfile("default");
}

function scr_destroy_character_profiles()
{
	ds_map_destroy(CHARPROF_MAPPING);
	
	if (fn_waitress != -1)
		font_delete(fn_waitress);
}

function scr_extract_character_name(_text)
{
	var split_text = scr_split(_text, ":");
	
	if ds_list_size(split_text) == 1
		return {name: "default", header_length: 0}
	else
	{
		var name = split_text[|0];
		var name_trimmed = scr_trim(name); 
		ds_list_destroy(split_text);
		
		var return_struct = {};
		return_struct.name = name_trimmed;
		return_struct.header_length = string_length(name) + 1;
		
		return return_struct;
	}
}
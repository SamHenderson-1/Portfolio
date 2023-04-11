// Script assets have changed for v2.3.0 see
// https://help.yoyogames.com/hc/en-us/articles/360005277377 for more information

/// @function				scr_create_globals()
/// @description			Creates the global events and parameters that can
///							be accessed by dialogue nodes.
function scr_create_globals(){
	
	// -- Events --
	global.date_events = ds_map_create();
	var _events = [ "DATE_JUMP",
	"INCREMENT_SPEED",
	"DECREMENT_SPEED",
	"SET_SPEED",
	"SAVE_SPEED",
	"LOAD_SPEED",
	"CREATE_PROPERTY",
	"SET_PROPERTY",
	"INCREMENT_PROPERTY",
	"DECREMENT_PROPERTY",
	"DEBUG", 
	"FOCUS",
	"END_FOCUS",
	"MAX",
	"SOUND",
	"SOUND_BLOCK",
	"SKIP_PAUSE",
	"WAITRESS_JUMP",
	"WAITRESS_ENTER",
	"WAITRESS_EXIT",
	"FADE_IN",
	"FADE_OUT",
	"FADE_FILL",
	"FADE_CLEAR",
	"WAITRESS_ENTER_BLOCK",
	"WAITRESS_EXIT_BLOCK",
	"FADE_IN_BLOCK",
	"FADE_OUT_BLOCK",
	"HYPERFIX",
	"MUSIC_START",
	"MUSIC_STOP"
	]
	var _event_count = array_length(_events);
	
	for(var i = 0; i < _event_count; i++)
		scr_register_global_event(_events[i])
	
	scr_set_static_events();

	// -- Properties --
	global.date_properties = ds_map_create();
	var _props = ["points",
	"saved_speed", 
	"max_result",
	"max_tie"
	];
	var _prop_count = array_length(_props);
	
	for (var i = 0; i < _prop_count; i++)
		scr_register_global_property(_props[i]);
	
	// Other Globals
	scr_create_sound_mapping();
	scr_create_character_profiles();
}


function DateProperty() constructor
{
	value = 0;
	on_change = new DateEvent();
	
	static set_value = function(_new_value)
	{
		value = _new_value;
		on_change.invoke(value);
	}
	
	static get_value = function()
	{
		return value;
	}
}

/// @function				scr_register_global_event()
/// @description			registers a global event with the given name.
/// @param {String} _name	The name of the event to register.
function scr_register_global_event(_name)
{
	ds_map_add(global.date_events, _name, new DateEvent());
}

function scr_register_global_property(_name)
{
	ds_map_add(global.date_properties, _name, new DateProperty());
}

/// @function				scr_global_event_exists(_name)
/// @description			Tells whether a global event exists.
/// @param {String} _name	The name of the event to check.
/// @returns {Bool}			Whether the string corresponds to a global event.
function scr_global_event_exists(_name)
{
	return ds_map_exists(global.date_events, _name);
}

function scr_global_property_exists(_name)
{
	return ds_map_exists(global.date_properties, _name);
}

/// @function				scr_global_get_event(_name)
/// @description			Gets the event that corresponds to a given name.
/// @param {String} _name	The name of the event to get.
/// @returns {DateEvent}	The global event that corresponds to the given
///							name.
function scr_global_get_event(_name)
{
	return global.date_events[?_name];
}

function scr_global_get_property_value(_name)
{
	return global.date_properties[?_name].get_value();
}

function scr_global_get_property_event(_name)
{
	return global.date_properties[?_name].on_change;
}

function scr_global_set_property_value(_name, value)
{
	global.date_properties[?_name].set_value(value);
}

/// @function				scr_destroy_globals()
/// @description			Destroys the global event and variable 
///							structures.
function scr_destroy_globals(){
	ds_map_destroy(global.date_events);
	ds_map_destroy(global.date_properties);
	// Other Globals
	scr_destroy_sound_mapping();
	scr_destroy_character_profiles();
}

// -- Static Events --


function scr_set_static_events()
{
	scr_global_get_event("CREATE_PROPERTY").add_listener(scr_register_global_property_dialogue, self);
	scr_global_get_event("SET_PROPERTY").add_listener(scr_global_set_property_value_dialogue, self);
	scr_global_get_event("INCREMENT_PROPERTY").add_listener(scr_global_increment_property_value_dialogue, self);
	scr_global_get_event("DECREMENT_PROPERTY").add_listener(scr_global_decrement_property_value_dialogue, self);
	scr_global_get_event("DEBUG").add_listener(scr_show_debug_message_dialogue, self);
	scr_global_get_event("MAX").add_listener(scr_tiebreaker, self);
	scr_global_get_event("SOUND").add_listener(scr_play_sound, self);
}

function scr_register_global_property_dialogue(_param)
{
	if (array_length(_param) < 1)
		throw("Argument Exception: scr_register_global_property_dialogue requires at least one parameter. Got none.");
	
	if (typeof(_param[0]) != "string")
		throw("Argument Exception: scr_register_global_property_dialogue requires the first parameter to be a string. Got a " + typeof(_param[0]) + ".");
	
	scr_register_global_property(_param[0]);
	
	if (array_length(_param) >= 2)
		scr_global_set_property_value(_param[0], _param[1]);
}

function scr_global_set_property_value_dialogue(_params)
{
	if (array_length(_params) < 2)
		throw("Argument Exception: scr_global_set_property_value_dialogue requires a string and a value. Got " + array_length(_params) + " parameters.");
	
	if (typeof(_params[0]) != "string")
		throw("Argument Exception: scr_global_set_property_value_dialogue requires the first parameter to be a string. Got a " + typeof(_params[0]) + ".\n\rDid you forget to add a * before a parameter?");
		
	scr_global_set_property_value(_params[0], _params[1])
}

function scr_global_increment_property_value_dialogue(_params)
{
	if (array_length(_params) < 1)
		throw("Argument Exception: scr_global_increment_property_value_dialogue requires a string. Got 0 parameters.");
	
	if (typeof(_params[0]) != "string")
		throw("Argument Exception: scr_global_increment_property_value_dialogue requires the first parameter to be a string. Got a " + typeof(_params[0]) + ".\n\rDid you forget to add a * before a parameter?");
	
	var increment = 1;
	
	if (array_length(_params) >= 2)
		increment = _params[1];
	
	var incremented_value = scr_global_get_property_value(_params[0]) + increment;
	scr_global_set_property_value(_params[0], incremented_value);
}

function scr_global_decrement_property_value_dialogue(_params)
{
	if (array_length(_params) < 1)
		throw("Argument Exception: scr_global_decrement_property_value_dialogue requires a string. Got 0 parameters.");
	
	if (typeof(_params[0]) != "string")
		throw("Argument Exception: scr_global_decrement_property_value_dialogue requires the first parameter to be a string. Got a " + typeof(_params[0]) + ".\n\rDid you forget to add a * before a parameter?");
	
	var decrement = 1;
	
	if (array_length(_params) >= 2)
		decrement = _params[1];
	
	var decremented_value = scr_global_get_property_value(_params[0]) - decrement;
	scr_global_set_property_value(_params[0], decremented_value);
}

function scr_show_debug_message_dialogue(_params)
{
	show_debug_message("<Dialogue Output>");

	
	var _param_count = array_length(_params);
	for (var i  = 0; i < _param_count; i++)
		show_debug_message(_params[i]);
	
	show_debug_message("</Dialogue Output>");
}
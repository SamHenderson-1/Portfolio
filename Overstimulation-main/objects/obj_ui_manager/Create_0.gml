/// @description				Initializes the UI Manager.


#region Private Methods

/// @function				next_page_wait()
/// @description			Sets a timer to move on to the next page, adding a breif pause
///							between pages. Typically called after finishing a page.
/// @self obj_ui_manager
function next_page_wait()
{
	var page_length = string_length(dialogue_node.pages[page_number]);
	dialogue_node.check_events(page_number, page_length, page_length + 1);
	
	if (! skip_pause)
		alarm[0] = room_speed * calculate_pause_time();
	else
	{
		skip_pause = false;
		next_page();
	}
}

/// @function				next_page()
/// @description			Moves this manager to the next page of dialogue. Clears the
///							speaker textbox before doing so. If the manager has run out
///							pages, initiates the response procedure (instantiate response
///							buttons and starts the response timer).
/// @self obj_ui_manager
function next_page()
{
	// If there are more pages of dialogue to read, read them.
	if (page_number < dialogue_node.page_count - 1)
	{
		// Read dialogue.
		page_number++;
		
		var _page = dialogue_node.pages[page_number];
		var _header_details = scr_extract_character_name(_page);
		var _character_name = _header_details.name;
		var _header_length = _header_details.header_length;
		
		set_character_profile(_character_name);
		
		if (_page == "")
		{
			dialogue_node.check_events(page_number, 0, 1);
			next_page();
		}
		else
		{
			if _header_length > 0
				dialogue_node.check_events(page_number, 0, _header_length);
			
			speaker_textbox.write_text(_page, _header_length);
			// If the speaker textbox was hidden from a response procedure, show it.
			if (! speaker_textbox.visible)
					speaker_textbox.visible = true;
		}
	}
	// If there are no more pages to read, and there are possible responses,
	// initiate response procedure.
	else
	{
		switch(dialogue_node.ending)
		{
			case Dialogue_Node_Ending.JUMP:
				retrieve_dialogue(dialogue_node.get_jump());
				break;
			case Dialogue_Node_Ending.RESPONSES:
				if (dialogue_node.response_count > 0)
				{
					// Hide the text box during responses.
					speaker_textbox.visible = false;
					// Show the response options.
					instantiate_buttons();
					// Set the time limit for responding.
					alarm[1] = room_speed * calculate_response_time();
				}
				else
					throw ("Warning: Crashing on response ending dialogue node " + dialogue_node.name + " with no responses.");
				break;
		}
	}
}

/// @function				retrieve_dialogue_button(_response_button_index)
/// @description			Callback for buttons to use when clicked. Records
///							the selected option and continues down the dialogue
///							tree accordingly.
/// @param {int64}	_response_button_index The index of the button in the current
///							dialogue node's list of responses.
/// @self obj_ui_manager
function retrieve_dialogue_button(_response_button_index)
{
	// Cancel the response time limit alarm.
	alarm[1] = -1;
	// Play a selection sound.
	scr_play_with_random_pitch(sfx_menu_click, 0.9, 1.1);
	// Continue down the dialogue tree.
	retrieve_dialogue(dialogue_node.response_ids[_response_button_index]);
}

/// @function				retrieve_dialogue(_response_id)
/// @description			Continues down the dialogue tree for the given
///							response. Called on Create with the _response_id
///							"start".
/// @param {String} _response_id The ID if the response. IDs of responses
///							are stored in dialogue nodes.
/// @self obj_ui_manager
function retrieve_dialogue(_response_id)
{
	// Turn off the hyperfixation mechanic in case it was turned on.
	hyperfixation_controller.set_active(false);
	hyperfixation_response_index = -1;
	
	// If we're not retrieving dialogue for the first time, we're calling
	// this method from a response procedure, thus we must destroy the buttons
	// we had up for the response procedure.
	if (variable_instance_exists(id, "dialogue_node"))
		destroy_buttons();
	
	// Get the next dialogue segment.
	dialogue_node = instance_find(o_story_manager, 0).get_dialogue_node(_response_id);
	
	// Show the first page of this new segment.
	page_number = -1;
	if dialogue_node != noone
		next_page();
	else
		end_event.invoke();
}

/// @function				instantiate_buttons()
/// @description			Creates the buttons for the current dialogue node's
///							responses. The button order on screen is randomized,
///							but the button objects are stored in the same order
///							in the manager's response_buttons array.
/// @self obj_ui_manager
function instantiate_buttons()
{
	// Create a random ordering of the buttons by shuffling their
	// indices.
	random_button_order = ds_list_create();
	for (var i = 0; i < dialogue_node.response_count; i++)
		ds_list_add(random_button_order, i);
	ds_list_shuffle(random_button_order);
	
	// For each response option, instantiate a button.
	for(var i = 0; i < dialogue_node.response_count; i++)
	{
		// Create button instance (position in screen is determined by manager's position).
		var _button = instance_create_layer(x,y - (130) * (random_button_order[| i] + 1), layer_get_id("UI"), obj_ui_textbutton);
		response_buttons[i] = _button;
		// Set button specs.
		_button.set_callbacks(self, i, _button.on_press, retrieve_dialogue_button, _button.on_up);
		_button.show_text(dialogue_node.responses[i]);
		_button.set_overflow_modes(Overflow_Mode.WRAP, Overflow_Mode.OVERFLOW);
		_button.resize(700, 75);
		
		if (i == hyperfixation_response_index)
		{
			hyperfixation_controller.set_target_button(_button);
			hyperfixation_controller.set_active(true);
		}
	}
}

/// @function				destroy_buttons()
/// @description			Destroys all the active buttons for a response
///							procedure.
/// @self obj_ui_manager
function destroy_buttons()
{
	// For each button, destroy it.
	for(var i  = 0; i < dialogue_node.response_count; i++)
	{
		var _button = response_buttons[i];
		with (_button)
		{
			instance_destroy();
		}
	}
	// Clear the response_buttons array.
	array_delete(response_buttons, 0, dialogue_node.response_count);
}

function increment_pacing_speed(_params = [])
{
	var _increment = 1;
	if array_length(_params) > 0
		_increment = _params[0];
	
	pacing_speed += _increment;
	on_pacing_speed_change.invoke();
}

function decrement_pacing_speed(_params = [])
{
	var _decrement = 1;
	if array_length(_params) > 0
		_decrement = _params[0];
	
	pacing_speed = max(0, pacing_speed - _decrement);
	on_pacing_speed_change.invoke();
}

function set_pacing_speed(_params)
{
	if (array_length(_params) == 0)
		throw ("Called set_pacing_speed with no parameters.");

	if (typeof(_params[0]) != "number")
		throw ("Called set_pacing_speed with non-real parameter " + string(_params[0]) + ".");
	
	pacing_speed = _params[0];
	on_pacing_speed_change.invoke();
}

function save_pacing_speed()
{
	scr_global_set_property_value("saved_speed", pacing_speed);
}

function load_pacing_speed()
{
	pacing_speed = scr_global_get_property_value("saved_speed");
	on_pacing_speed_change.invoke();
}

function set_speaker_text_box_speed()
{
	speaker_textbox.set_write_rate(calculate_writing_speed());
}

/// @function				calculate_writing_speed()
/// @description			Calculates the speed at which the date talks
///							based off the number of dialogue segments
///							(nodes in the dialogue tree) traversed.
/// @self obj_ui_manager
function calculate_writing_speed()
{
	return (30 + 15 * pacing_speed) / 2;
}

/// @function				calculate_response_time()
/// @description			Calculates the time limit for responding
///							based off the number of dialogue segments
///							(nodes in the dialogue tree) traversed.
/// @self obj_ui_manager
function calculate_response_time()
{
	var high = 10;
	var low = 4;
	var halfspeed_control = 8;
	
	return (high+low)/2 - (high-low)/pi*arctan(pacing_speed / halfspeed_control);
}

/// @function				calculate_pause_time()
/// @description			Calculates the length of pauses between pages
///							based off the number of dialogue segments
///							(nodes in the dialogue tree) traversed.
/// @self obj_ui_manager
function calculate_pause_time()
{
	var high = 5;
	var low = 1;
	var halfspeed_control = 3;
	
	return (high+low)/2 - (high-low)/pi*arctan(pacing_speed / halfspeed_control);
}

/// @function				play_talking_sound()
/// @description			Callback for the speaker text box. Whenever
///							a character is written, talking dialogue is
///							played.
/// @self obj_ui_manager
function play_talking_sound(_last_progress, _current_progress)
{
	if (dialogue_node != noone)
		dialogue_node.check_events(page_number, _last_progress, _current_progress);
	
	if (! audio_is_playing(dialogue_voice))
	{
		current_voice_audio_playing = scr_play_with_random_pitch(dialogue_voice, 0.9, 1.1, 25);
	}
}

function pause() {
	paused = !paused;
	if (paused) {
		pauseVal0 = alarm[0];
		alarm[0] = -1;
		pauseVal1 = alarm[1];
		alarm[1] = -1;
	}
	else
	{
		alarm[0] = pauseVal0;
		alarm[1] = pauseVal1;
	}
}

function play_blocking_sound(args)
{
	var blocking_sound = scr_play_sound(args);
	ds_list_add(blocking_sounds, blocking_sound);
	block();
}

function block()
{
	block_count++;
	if (block_count > 0)
		speaker_textbox.block();
}

function unblock()
{
	block_count--;
	if (block_count <= 0)
		speaker_textbox.unblock();
}

function set_skip_pause()
{
	skip_pause = true;
}

function animate_end()
{
	var animation = instance_create_layer(0,0, layer_get_id("UI"), obj_ending_animation);
	animation.fade_out_end.add_listener(set_backdrop_for_titlecard, self);
	animation.animation_end.add_listener(return_to_start_menu, self);
}

function set_backdrop_for_titlecard()
{
	speaker_textbox.visible = false;
	scr_global_get_event("FADE_FILL").invoke();
}

function return_to_start_menu()
{
	room_goto(rm_StartMenu);
}

function set_character_profile(_character_name)
{
	var profile = scr_get_character_profile(_character_name);
	dialogue_voice = profile.speech_audio;
	speaker_textbox.set_font(profile.font);
	
	if (current_voice_audio_playing != -1 and audio_is_playing(current_voice_audio_playing))
		audio_stop_sound(current_voice_audio_playing);
}

function set_hyperfixation_response(args)
{
	if (array_length(args) < 1)
		throw("HYPERFIX requires at least one parameter, a hyperfixation index.")
	
	hyperfixation_response_index = args[0] - 1;
	
	var _intensity = 0.1667;
	
	if (array_length(args) > 1)
		_intensity = args[1];
	
	hyperfixation_controller.set_intensity(_intensity);
}

#endregion

#region Initialization

// speaker textbox
speaker_textbox = instance_create_layer(x,y,layer_get_id("UI"),obj_ui_speaking_textbox);
speaker_textbox.set_finish_callback(self, next_page_wait);
speaker_textbox.set_letter_callback(self, play_talking_sound);
speaker_textbox.resize(1024, 120);

pacing_speed = 0;
on_pacing_speed_change = new DateEvent();
on_pacing_speed_change.add_listener(set_speaker_text_box_speed, self);
on_pacing_speed_change.invoke();

// dialogue responses
response_buttons[0] = noone;
pauseVal0 = 0;
pauseVal1 = 0;
paused = false;

// block count is a counter for the number of processes that are blocking the
// dialogue from continuing.
block_count = 0;

// blocking sound stopped the game from progressing until the specified sound has
// stopped playing
blocking_sounds = ds_list_create();

// if skip_pause is true, there won't be a delay between the next two pages of
// dialogue.
skip_pause = false;

// dialogue initialization now happens in Room Start.

// end_event is invoked when the end of dialogue has been reached.
end_event = new DateEvent();
end_event.add_listener(animate_end, self);

// Character Profile Information
dialogue_voice = sfx_dialogue_voice;
current_voice_audio_playing = -1;

// Hyperfixation Controller
hyperfixation_controller = instance_find(obj_hyperfixation, 0);
hyperfixation_response_index = -1;

#endregion

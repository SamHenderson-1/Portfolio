/// @function				scr_game_text(_text_id, _dialogue_node)
/// @description			Returns the dialogue node for a given
///							choice.
/// @param {String} _text_id The id of the previous response.
/// @param {DialogueNode} _dialogue_node The dialogue node to
///							build off of. If called from outside
///							of the method, don't set this parameter.
/// @deprecated
function scr_game_text(_text_id, _dialogue_node = new DialogueNode(_text_id)) {
	// Maps the _text_id to sequences of pages
	// and responses.
	switch(_text_id) {
		case "start":
			// "start" is the initial _text_id.
			_dialogue_node.add_page("So... isn't it just the best here? Y'know, I always save this place for people I really like. It's an atmosphere you can't find in any other place. I don't know how to describe it... Why don't you do it for me? Whaddya think of this place?");
			_dialogue_node.add_response("I think it's great!", "1 - great");
			_dialogue_node.add_response("It's kind of loud.", "1 - loud");
			_dialogue_node.add_response("I think I'll get the breadsticks.", "1 - breadsticks");
			_dialogue_node.add_response("Hmm...", "1 - hmm");
			break;
		case "1 - great":
			_dialogue_node.add_page("Well, I'm glad you like it. A man of my taste is one I can get along with.");
			_dialogue_node.set_jump("2 - step");
			break;
		case "1 - loud":
			_dialogue_node.add_page("Oh, yeah I guess so, I'm just a big fan of this scene.");
			_dialogue_node.set_jump("2 - step");
			break;
		case "1 - breadsticks":
			_dialogue_node.add_page("Huh? Okay... I think I'll get us a round of drinks, sound good?");
			_dialogue_node.set_jump("2 - step");
			break;
		case "1 - hmm":
			_dialogue_node.add_page("...oh, you look sorta lost in thought. Guess it's a lot to take in, huh.");
			_dialogue_node.set_jump("2 - step");
			break;
		case "2 - step":
			_dialogue_node.add_page("Well anyways, I'm interested in what you're about. Do you have a favorite drink? You more of a fancy glass or a rough tankard kind of guy? Me? I'm more of a whiskey on the rocks with a splash of coke. Basic, but it helps it go down easier. But I'll order whatever you want to get.");
			_dialogue_node.add_response("What about ginger beer with some lime instead?", "2 - ginger lime");
			_dialogue_node.add_response("I just like water.", "2 - water");
			_dialogue_node.add_response("I love rocks, I've got a huge collection back at home!", "2 - rocks");
			_dialogue_node.add_response("I'm not sure...", "2 - not sure");
			break;
		case "2 - ginger lime":
			_dialogue_node.add_page("Wow, I'm impressed. That's a great choice, you really know your stuff when it comes to alcohol.");
			_dialogue_node.set_jump("3 - step");
			break;
		case "2 - water":
			_dialogue_node.add_page("Alright, I mean that's more vanilla than actual vanilla but I won't judge.");
			_dialogue_node.set_jump("3 - step");
			break;
		case "2 - rocks":
			_dialogue_node.add_page("Oh... well that's something. Rocks aren't really my favorite, crystals and gems are good though.");
			_dialogue_node.set_jump("3 - step");
			break;
		case "2 - not sure":
			_dialogue_node.add_page("It's kinda hard to decide. There's lots of them to choose from though, that's for sure...");
			_dialogue_node.set_jump("3 - step");
			break;
		case "3 - step":
			_dialogue_node.add_page("Oh, Looks like our drinks are here...");
			_dialogue_node.add_page("Waitress: I've got two drinks coming right up! First round is on the house, good lookin' folk like this young fellow sitting across from you actually deserve it after all...");
			_dialogue_node.add_page("Waitress: Who's this one? Weren't you in here last week wit-");
			_dialogue_node.add_page("H-Hey now, let's not go into that old friend...");
			_dialogue_node.add_page("Waitress: Who're you callin' old huh? I pulled more men last week than you have in the past month and I work a full time job. Beats you at least, drunk fool.");
			_dialogue_node.add_page("Quality over quantity. I may be a man of the town but you know I can afford to pick carefully. This guy's an absolute charmer and you can't deny that. Whaddya think, am I right?");
			_dialogue_node.add_response("I mean, you are kind of haggard, and you definitely drink like an old sailor...", "3 - sailor");
			_dialogue_node.add_response("I mean it's not really like either of you should be bragging about all that anyways. People aren't just objects in your little games, you know.", "3 - object");
			_dialogue_node.add_response("I don't feel like I pulled anything. My legs are fine.", "3 - leg");
			_dialogue_node.add_response("I mean...", "3 - mean");
			break;
		case "3 - sailor":
			_dialogue_node.add_page("Haah, So I'm a pirate now? I kind of like that image... It's quite fitting for my acquired taste and rugged physique. Thanks for the compliment.");
			_dialogue_node.set_jump("4 - step");
			break;
		case "3 - object":
			_dialogue_node.add_page("...oh. No need to be so serious man, I was just pulling your leg.");
			_dialogue_node.set_jump("4 - step");
			break;
		case "3 - leg":
			_dialogue_node.add_page("Oh... That's good...?");
			_dialogue_node.add_page("I feel like I'm missing a joke here...");
			_dialogue_node.set_jump("4 - step");
			break;
		case "3 - mean":
			_dialogue_node.add_page("...Well your silence speaks volumes, I'm clearly right.");
			_dialogue_node.set_jump("4 - step");
			break;
		case "4 - step":
			_dialogue_node.add_page("Waitress: Ugh, y'know you could stand to lose some of that cockiness, hun. It'd serve you well when talking to a lady, that's for sure...");
			_dialogue_node.add_page("Haah, it was always like you to undermine my greatest features... Well, I'm sure you've got other customers to bother, don't-cha? It was nice introducing you two though.");
			_dialogue_node.add_page("Waitress: Well take care now you... I'm serious about that! No more late nights, and don't drink yourself into a coma. I don't want to see it on our tab here.");
			_dialogue_node.add_page("Okay... I won't, I promise. I'm 30 somethin' now y'know, don't go treating me like a kid.");
			_dialogue_node.add_page("Anyways, how'd you like my ride tonight? It's the oldest model, a Red Cadillac in pretty good condition if you ask me.");
			_dialogue_node.add_response("Yeah, the leather seats were comfy. Surprisingly no stains either.", "4 - leather");
			_dialogue_node.add_response("The... convertible? Yeah it was nice, I always wanted one myself.", "4 - convertible");
			_dialogue_node.add_response("I mean something like that hasn't happened yet...", "4 - yet");
			_dialogue_node.add_response("Right... Yeah!", "4 - yeah");
			break;
		case "4 - leather":
			_dialogue_node.add_page("You think? I got them custom made with a revamped A/C system and a killer speaker set just for nights like these. Installed new headlights because the old ones were tacky, but I like older stuff.");
			_dialogue_node.set_jump("5 - step");
			break;
		case "4 - convertible":
			_dialogue_node.add_page("Huh? What do you mean 'convertible'? Okay, there are some great convertible models out there, but this is not one of them. Could you not tell? Thought you had some taste...");
			_dialogue_node.set_jump("5 - step");
			break;
		case "4 - yet":
			_dialogue_node.add_page("Wait what? Oh, I see what you did there... I think? Seems like I'm missing a lot of jokes tonight for some reason.");
			_dialogue_node.set_jump("5 - step");
			break;
		case "4 - yeah":
			_dialogue_node.add_page("Are you... even paying attention? I think you might've missed my question or something...");
			_dialogue_node.set_jump("5 - step");
			break;
		case "5 - step":
			_dialogue_node.add_page("Alright, here comes the food! I ordered us some pizza and poppers. It's kind of all they have here really, but it's honestly the best food you'll find in town. I get meat lovers with a side of ranch for dipping, hope that's okay?");
			_dialogue_node.add_response("Can I dip my poppers in the ranch too?", "5 - dip");
			_dialogue_node.add_response("Sorry, I'm kind of a picky eater. I think I'll have something else when I get home...", "5 - picky");
			_dialogue_node.add_response("Poppers...? Like-", "5 - poppers");
			_dialogue_node.add_response("Oh.. Yeah, that's fine.", "5 - fine");
			break;
		case "5 - dip":
			_dialogue_node.add_page("Ooh, adventurous. Always have loved experimental eaters, people who get super particular about their food choices bother me sometimes. Like eat what's in front of you, even if you end up not likin' it. You might end up regretting it later when you don't have something to eat...");
			_dialogue_node.set_jump("6 - step");
			break;
		case "5 - picky":
			_dialogue_node.add_page("I see. You don't want the food? I'll just go tell 'em to bring it back then. No use in eating alone, is there?");
			_dialogue_node.set_jump("6 - step");
			break;
		case "5 - poppers":
			_dialogue_node.add_page("Huh? Oh, nope. Not like that. Funny though, I've never actually tried that kind before.");
			_dialogue_node.set_jump("6 - step");
			break;
		case "5 - fine":
			_dialogue_node.add_page("Okay...? Did you want something else? You don't sound very enthused to me.");
			_dialogue_node.set_jump("6 - step");
			break;
		case "6 - step":
			_dialogue_node.add_page("You go home together, he drops you off with a kiss goodbye, and you feel like maybe this guy will be the one to finally work out for you.");
			break;
	}
	
	return _dialogue_node;
}

/// @function				DialogueEventCall(_page, _index_in_text, _event_name)
/// @description			Creates a dialogue event call in the
///							middle of a page of dialogue.
/// @param {int64} _page	The page number the event exists on.
/// @param {int64} _index_in_text Where in the page the event exists.
/// @param {String} _event_name The name of the event to be invoked when
///							this call is reached while reading dialogue.
/// @param {Array} _parameters The parameters to be passed to the inovked
///							event.
function DialogueEventCall(_page, _index_in_text, _event_name, _parameters = []) constructor
{
	page = _page;
	index_in_text = _index_in_text;
	event_name = _event_name;
	parameters = _parameters;
	
	/// @function			check_range(_page, _start, _end)
	/// @description		Checks whether this dialogue event is in
	///						the passed range (called when reading 
	///						dialogue).
	/// @param {int64} _page The page that is currently being read.
	/// @param {int64} _start The index in the page at the start of
	///						read range.
	/// @param {int 64} _end The index in the page at the end of the
	///						read range.
	/// @returns {Bool}		True if this DialogueEventCall is in the
	///						given range. False otherwise.
	static check_range = function(_page, _start, _end)
	{
		var _same_page = _page == page;
		var _in_text_range = index_in_text >= _start and index_in_text < _end;
		
		return _same_page and _in_text_range;
	}
	
	/// @function			invoke()
	/// @description		Invoke global event of this 
	///						DialogueEventCall.
	static invoke = function()
	{
		var _evaluated_parameters = [];
		
		for (var i = 0; i < array_length(parameters); i++)
			_evaluated_parameters[i] = parameters[i].evaluate();
		
		scr_global_get_event(event_name).invoke(_evaluated_parameters);
	}
}

enum Dialogue_Node_Ending {JUMP, RESPONSES}

/// @function				DialogueNode()
/// @description			Creates an empty dialogue node.
function DialogueNode(_name) constructor
{
	static event_start = "[";
	static event_end = "]";
	static event_separator = ",";
	
	name = _name;
	// Pages in this dialogue segment.
	page_count = 0;
	pages[0] = "";
	// -- Ending --
	ending = Dialogue_Node_Ending.JUMP;
	// Section to jump to at end of segment.
	jump_count = 0;
	jump_sections = [];
	jump_conditionals = [];
	// Responses at the end of this dialogue segment.
	response_count = 0;
	responses[0] = "";
	response_ids[0] = "";
	
	// -- Events --
	event_count = 0;
	events = [];
	
	/// @function			add_page(_page)
	/// @description		Adds a page to the end of this dialogue node.
	///						If there are [] brackets in the page, the enclosed
	///						content will be treated as events.
	/// @param {String} _page The page to add to this portion of dialogue.
	static add_page = function(_page)
	{
		// Events can be called in the middle of dialogue.
		// _text_count keeps track of where in the dialogue
		// we are at.
		var _text_count = 0;
		// The true text that will be written, ignoring the
		// contents of [] brackets.
		var _final_display = "";
		// The page split by [ characters. The 0th element
		// will never include event calls. The following
		// elements always start with event calls.
		var _split_by_start = scr_split(_page, event_start);
		var _split_length = ds_list_size(_split_by_start);
		
		// Register the first part of the page...
		var _text_segment = _split_by_start[|0];
		_text_count += string_length(_text_segment);
		_final_display += _text_segment;
		
		// Register the events and text of the following,
		// parts.
		for (var  i = 1; i < _split_length; i++)
		{
			_text_segment = _split_by_start[|i];
			
			// Separate event calls from text to display.
			var _split_by_end = scr_split(_text_segment, event_end);
			var _pre_and_post_len = ds_list_size(_split_by_end);
			// If there was no split, or more than 1, then
			// there's not a [ for every ] or vice versa.
			if (_pre_and_post_len < 2)
				throw ("Expected event closing symbol " + event_end + " for event section number " + string(i) + ": " + _text_segment + ".");
			else if (_pre_and_post_len > 2)
				throw ("Too many event closing symbols " + event_end + " for event section number " + string(i) + ": " + _text_segment + ".");
			
			// Event calls.
			var _events = _split_by_end[| 0];
			// Text to display.
			var _post_events = _split_by_end[| 1];
				
			// Register Events
			var _split_events = scr_split(_events, event_separator);
			var _split_event_count = ds_list_size(_split_events);
			
			var _event_str = "";
			
			// Parse events individually. "," also splits parameters,
			// so we have to check whether a string actually contains
			// an event and all its parameters.
			for (var e = 0; e < _split_event_count; e++)
			{
				// Get the current event string.
				if _event_str == ""
					_event_str += _split_events[|e];
				// If the last event string didn't finish its parameters,
				// then this is a parameter in the same event as the last one.
				else
					_event_str += "," + _split_events[|e];
				
				var _parameters_open = (string_pos("(", _event_str) != 0);
				var _parameters_closed = (string_pos(")", _event_str) != 0);
				// If the parameters for this event aren't done, we need to
				// concatenate the next string.
				if _parameters_open xor _parameters_closed
					continue;
				// -- Beyond this point we have a complete event w/ parameters --
				_event_str = scr_trim(_event_str);
				
				var _event_and_params = scr_try_parse_event(_event_str);
				// If the event and its parameters couldn't be parsed, throw an exception.
				if _event_and_params == noone
					throw ("Unable to parse event " + _event_str + " for event section number " + string(i) + ": " + _text_segment + ".");

				// Extract the events and parameters.
				var _event = _event_and_params[0];
				var _params = _event_and_params[1];
				// If the event isn't recognized by the game, throw an exception.
				if not scr_global_event_exists(_event)
					throw ("Non-existent event " + _event + " for event section number " + string(i) + ": " + _text_segment + ".");
				// -- At this point, we know our event is properly structured and can register it --
				events[event_count] = new DialogueEventCall(page_count, _text_count, _event, _params);
				event_count++;
				// Reset _event_str as we will be starting a new event.
				_event_str = "";
			}
			// If _event_str ended with contents, the last event of a section was malformed.
			if (_event_str != "")
				throw ("Last event " + _event_str + " missing parameter parenthesis for event section: " + _text_segment + ".");
				
			// Add Post-Events Text
			_text_count += string_length(_post_events);
			_final_display += _post_events;
			
			// Cleanup
			ds_list_destroy(_split_events);
			ds_list_destroy(_split_by_end);
		}
		// Cleanup
		ds_list_destroy(_split_by_start);
		
		pages[page_count] = _final_display;
		page_count++;
	}
	// Add a response to this dialogue segment.
	static add_response = function(_response, _response_id)
	{
		if (ending == Dialogue_Node_Ending.JUMP)
		{
			if (jump_count > 0)
				show_debug_message("Warning: Dialogue node " + name + " has conflicting endings.");
			ending = Dialogue_Node_Ending.RESPONSES;
		}
		
		responses[response_count] = _response;
		response_ids[response_count] = _response_id;
		response_count++;
	}
	/// @function			set_jump(_to_jump)
	/// @description		Sets the unconditional jump for this
	///						node.
	/// @param {String} _to_jump The name of the next dialogue
	///						node to jump to.
	static add_jump = function(_to_jump, _conditional = new ParsedValue("1"))
	{
		if (ending == Dialogue_Node_Ending.RESPONSES)
		{
			if (response_count > 0)
				show_debug_message("Warning: Dialogue node " + name + " has conflicting endings.");
			ending = Dialogue_Node_Ending.JUMP;
		}
		
		jump_sections[jump_count] = _to_jump;
		jump_conditionals[jump_count] = _conditional;
		jump_count++;
	}
	/// @function			check_events(_page, _start, _end)
	/// @description		Checks whether any of the dialogue node's
	///						events need to be invoked. An event
	///						can be invoked when it is on the page that
	///						matched _page and is between [_start, _end)
	///						where _start and _end are between [0, PAGE LENGTH].
	/// @param {int64} _page The page that we're checking.
	/// @param {int64} _start The start index of the range to check.
	/// @param {int64} _end The end index of the range to check.
	static check_events = function(_page, _start, _end)
	{
		for(var i = 0; i < event_count; i++)
		{
			var _event = events[i]; 
			if(_event.check_range(_page, _start, _end))
				_event.invoke();
		}
	}
	
	static get_jump = function()
	{
		for(var i = 0; i < jump_count; i++)
		{
			if (jump_conditionals[i].evaluate())
				return jump_sections[i];
		}
		
		return "";
	}
}
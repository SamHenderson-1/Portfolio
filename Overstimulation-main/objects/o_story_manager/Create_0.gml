/// @description Crafts and tracks progression in dialogue tree.

#region Public Methods
function get_dialogue_node(_node_name)
{
	if (_node_name == "")
		return noone;
	
	if not ds_map_exists(dialogue_node_map, _node_name)
		throw ("Received request for non-existent dialogue node " + _node_name + ".");
	return ds_map_find_value(dialogue_node_map, _node_name);
}
#endregion

#region Initialization

// Set Events Up
scr_create_globals();

// Set Dialogue Up
fname = "gold_storyline.txt";
dialogue_node_map = scr_open_file(fname);

#endregion
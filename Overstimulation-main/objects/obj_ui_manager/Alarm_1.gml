/// @description Forces a response to be picked during a dialogue sequence
/// when the player has run out of time.


if (dialogue_node.response_count > 0)
	retrieve_dialogue(dialogue_node.response_ids[dialogue_node.response_count - 1]);

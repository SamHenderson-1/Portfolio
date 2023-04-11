// Script assets have changed for v2.3.0 see
// https://help.yoyogames.com/hc/en-us/articles/360005277377 for more information

#macro BLOCK_DIALOGUE scr_block_dialogue();
#macro UNBLOCK_DIALOGUE scr_unblock_dialogue();

function scr_block_dialogue() {
	var ui_manager = instance_find(obj_ui_manager, 0);
	ui_manager.block();
}

function scr_unblock_dialogue() {
	var ui_manager = instance_find(obj_ui_manager, 0);
	ui_manager.unblock();	
}
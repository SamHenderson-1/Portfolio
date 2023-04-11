function scr_menu(argument0) {
	switch (argument0) {
		case 0: room_goto(rm_PrototypeUI); break; //Continue
		case 1: room_goto(rm_Credits); break; //Credits
		case 2: game_end(); break; //Exit
		case 3: room_goto(rm_StartMenu); break; //StartMenu
	}
}
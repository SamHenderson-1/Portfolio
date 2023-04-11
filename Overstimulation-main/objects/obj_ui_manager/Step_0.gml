/// @description Insert description here
// You can write your code in this editor
if(!paused){
	for (var i = ds_list_size(blocking_sounds) - 1; i >= 0 ; i--)
	{
		var blocking_sound = blocking_sounds[|i];
		if (!audio_is_playing(blocking_sound))
		{
			unblock();
			ds_list_delete(blocking_sounds, i);
		}
	}
}


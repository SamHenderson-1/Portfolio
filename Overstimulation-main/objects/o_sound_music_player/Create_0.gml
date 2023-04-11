/// @description Plays music on loop.

#macro TOTAL_GAIN 0.25
#macro FADE_DURATION (3 * room_speed)

#region Private Methods

function start_music()
{
	is_on = true;
	audio_start_sync_group(sync_group);
	audio_sound_gain(music_bar, 0, 0);
	audio_sound_gain(music_bar_focus, 0, 0);
	alarm[0] = FADE_DURATION;
}

function end_music()
{
	is_on = false;
	alarm[0] = FADE_DURATION;
}

function update_music()
{
	
	var gain;
	
	if (is_on)
	{
		if (alarm[0] > -1)
			gain = lerp(TOTAL_GAIN, 0, alarm[0] / FADE_DURATION);
		else
			gain = TOTAL_GAIN;
	}
	else
	{
		if (alarm[0] > -1)
			gain = lerp(0, TOTAL_GAIN, alarm[0] / FADE_DURATION);
		else
			gain = 0;
	}
	
	var muffled_percent = abs(0.5 - global.focus_plane_depth) * 2;
	var normal_percent = 1 - muffled_percent;
	
	audio_sound_gain(music_bar, normal_percent * gain, 0);
	audio_sound_gain(music_bar_focus, muffled_percent * gain, 0);
}

#endregion

#region Initialization

sync_group = audio_create_sync_group(true);
audio_play_in_sync_group(sync_group, music_bar);
audio_play_in_sync_group(sync_group, music_bar_focus);

is_on = false;

music_fade_end = new DateEvent();

#endregion
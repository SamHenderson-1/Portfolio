/// @function				scr_play_with_random_pitch(_sound_id, _min_pitch, _max_pitch, _priority, _loop)
/// @description			Plays the selected audio with a random 
///							pitch in the given range.
/// @param {Sound ID} _sound_id The ID of the sound to play.
/// @param {Real} _min_pitch The minimum pitch multiplier to choose from.
/// @param {Real} _max_pitch The maximum pitch multiplier to choose from.
/// @param {Real} _priority	The priority of the pitch to play.
/// @param {Bool} _loop		Whether to play the sound on loop.
/// @returns {Sound}		The sound played.
function scr_play_with_random_pitch(_sound_id, _min_pitch, _max_pitch, _priority = 50, _loop = false){
	var _pitch_diff = _max_pitch - _min_pitch;
	var _sound = audio_play_sound(_sound_id, _priority, _loop);
	audio_sound_pitch(_sound, _min_pitch + random(_pitch_diff));
	return _sound;
}
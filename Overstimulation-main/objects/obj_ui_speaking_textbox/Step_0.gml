/// @description Increments progress over time.

// Don't progress if blocked.
if (!blocked)
{
	if(!paused) {
		// Increment progress.
		var _dt = delta_time / 1000000;
		var _last_progress = progress;
		progress += _dt * write_rate;
		progress = clamp(progress, 0, to_write_len);
		
		// Show substring corresponding to progress.
		show_text(string_copy(to_write, 0, floor(progress)), to_write);

		// If we wrote a character, invoke the appropriate callback.
		if (floor(_last_progress) != floor(progress) or (to_write_len == 0 and not finished))
		{
			var _last_progress_floored = floor(_last_progress);
			var _current_progress_floored = floor(progress);			
			var i = _last_progress_floored;
			
			for (; i < _current_progress_floored; i++)
			{		
				var _to_call = on_letter_written;
				with (callback_object_on_letter)
				{
					_to_call(i, i + 1);
				}
				// Check if the last call blocked the speaker textbox.
				// If so, we need to pause the next callbacks and progress.
				if (blocked)
				{
					progress = i + 1;
					show_text(string_copy(to_write, 0, i), to_write);
					break;
				}
			}
		}
		// If we've reached the end of our text to write, invoke
		// the appropriate callback.
		if ((not finished) and progress == to_write_len)
		{
			finished = true;
			finish_signal.s_trigger();
		}	
	}
}

/// @description Insert description here
// You can write your code in this editor


if (FADE_ALARM > 0)
{
	switch(fade_state)
	{
		case FadeState.IN:
			opacity = FADE_ALARM / fade_duration;
			break;
		case FadeState.OUT:
			opacity = 1 - (FADE_ALARM / fade_duration);
			break;
	}
}




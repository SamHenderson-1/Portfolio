/// @description Insert description here
// You can write your code in this editor


if (alarm[1] > 0)
{
	switch(slide_state)
	{
		case SlideState.ENTERING:
			scr_slide_smooth(onscreen_x,y,offscreen_x,y, alarm[1] / slide_duration)
			break;
		case SlideState.EXITING:
			scr_slide_smooth(offscreen_x,y,onscreen_x,y, alarm[1] / slide_duration);
			break;
	}
}




// Inherit the parent event
if(!paused) {
	event_inherited(); 
	}


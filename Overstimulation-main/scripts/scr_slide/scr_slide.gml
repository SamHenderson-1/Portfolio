// Script assets have changed for v2.3.0 see
// https://help.yoyogames.com/hc/en-us/articles/360005277377 for more information
function scr_slide(_startx, _starty, _endx, _endy, _progress){
	x = lerp(_startx, _endx, _progress);
	y = lerp(_starty, _endy, _progress);
}

function scr_slide_smooth(_startx, _starty, _endx, _endy, _progress)
{
	_progress = 3 * (_progress * _progress) - 2 * (_progress * _progress * _progress);
	scr_slide(_startx, _starty, _endx, _endy, _progress);
}
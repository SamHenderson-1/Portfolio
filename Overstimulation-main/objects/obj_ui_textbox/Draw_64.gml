/// @description Draws the text in the box.

// Set draw parameters.
draw_set_color(color);
draw_set_font(font);
draw_set_halign(h_alignment);
draw_set_valign(v_alignment);

// Concatenate lines into a single string.
var final_string = "";
for (var i = ds_list_size(lines) - 1; i >= 0; i--)
{
	if (i == ds_list_size(lines) - 1)
		final_string = lines[| i];
	else
		final_string = lines[| i] + "\n" + final_string;
}

// Determine the x and y positions for the text
// in the box based on alignment.
var text_x = 0;
var text_y = 0;

switch(h_alignment)
{
	case fa_left:
		text_x = x;
		break;
	case fa_middle:
		text_x = x + width / 2;
		break;
	case fa_right:
		text_x = x + width;
		break;
}

switch(v_alignment)
{
	case fa_top:
		text_y = y;
		break;
	case fa_middle:
		text_y = y + height / 2;
		break;
	case fa_bottom:
		text_y = y + height;
		break;
}

// Draw text.
draw_text(text_x, text_y, final_string);
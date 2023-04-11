/// @description Insert description here
// You can write your code in this editor


#macro NEVADA_CREDITS "Nevada Black:\nConcept Art and UI Art\nProducer\nStoryboarding/Script writing\nSFX"
#macro WILLIAM_CREDITS "William Erignac:\nLead Programmer\nProducer\nGame Designer\nUI Design"
#macro SAM_CREDITS "Sam Henderson:\nProgrammer\nGame Designer\nUI Design"
#macro YANXIA_CREDITS "Yanxia Bu:\nBackground Art\nSprite Art\nGame Designer"
#macro ADDITIONAL_CREDITS "Special Thanks to Zapsplat.com for SFX/Music assets, Gaming Reverends for the base blur shader implementation, and Single Day Regular font by DXKorea on fontlibrary.org"

#macro AUTISM_RESOURCES "Autism Resources:\nNational Autistic Society - www.autism.org.uk\nAutistic Self Advocacy Network - autisticadvocacy.org\nAutistic Women and Non-BinaryNetwork - awnnetwork.org\nSelf Advocates Becoming Empowered - www.sabeusa.org"

draw_sprite_tiled(spr_menu, sub_image, x, y);

var middle_screen = display_get_gui_width()/2;

var first_column = display_get_gui_width()/5;
var second_column = display_get_gui_width()/2;
var resources_column = display_get_gui_width()*2/3;

var center_credits = display_get_gui_height()*4/9;
var references_height = display_get_gui_height()*9/10;

draw_set_font(fn_standard);
draw_set_halign(fa_middle);
draw_set_valign(fa_center);

draw_set_color(c_white);
draw_text(first_column, center_credits, NEVADA_CREDITS + "\n\n\n" + WILLIAM_CREDITS);
draw_text(second_column, center_credits, SAM_CREDITS + "\n\n\n" + YANXIA_CREDITS);
draw_set_halign(fa_left);
draw_text_ext(resources_column, center_credits, AUTISM_RESOURCES, 50, display_get_gui_width() * 1 / 4);

draw_set_halign(fa_middle);
draw_set_font(fn_additional_credits);
draw_text_ext(middle_screen, references_height, ADDITIONAL_CREDITS, 20, display_get_gui_width() * 5 / 6);


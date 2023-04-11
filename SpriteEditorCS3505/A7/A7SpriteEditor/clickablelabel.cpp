/*
Ben Rose, Grange Simpson, Tracy Garner, Krish Mahtani, Sam Henderson
CS3505
A7: Qt Sprite Editor

This file subclasses QLabel and makes a clickable QLabel
 */
#include "clickablelabel.h"
#include <QDebug>

///@brief initializes the clickable label
/// @param QWidget *parent
/// @param WindowFlags f
ClickableLabel::ClickableLabel(QWidget *parent, Qt::WindowFlags f)
    : QLabel(parent)
{
    setMouseTracking(true);
    mousePressed = false;
}

///@brief destructor
ClickableLabel::~ClickableLabel() {}

///@brief this method emits a MouseMoved signal when the event is triggered
/// @param QMouseEvent *event - a pointer for the mouse event
/// @note mousePressed is not set in this method, it sends the current state of mousePressed
void ClickableLabel::mouseMoveEvent(QMouseEvent *event){
    emit mouseMoved(event, mousePressed);
}

///@brief this method emits a MouseMoved signal when the event is triggered
/// @param QMouseEvent *event - a pointer for the mouse event
/// @note mousePressed is set to true, that value is passed to MouseMoved
void ClickableLabel::mousePressEvent(QMouseEvent *event){
    mousePressed = true;
    emit mouseMoved(event, mousePressed);
}

///@brief this method sets the mousePressed variable to false
/// @param QMouseEvent *event - a pointer for the mouse event
void ClickableLabel::mouseReleaseEvent(QMouseEvent *event) {
    mousePressed = false;
}


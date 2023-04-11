/*
Ben Rose, Grange Simpson, Tracy Garner, Krish Mahtani, Sam Henderson
CS3505
A7: Qt Sprite Editor

This file sets up all methods and variables needed for clickablelabel.cpp
 */
#ifndef CLICKABLELABEL_H
#define CLICKABLELABEL_H

#include <QLabel>
#include <QMouseEvent>
#include <QWidget>
#include <QString>
#include <Qt>
#include <QPixmap>

///@brief This class extends the features of the QLabel to include mouse functionality including coordinate tracking
///@inherits QLabel
class ClickableLabel : public QLabel
{
    Q_OBJECT
public:
    //default constructor
    explicit ClickableLabel(QWidget *parent = nullptr, Qt::WindowFlags f = Qt::WindowFlags());
    //destructor
    ~ClickableLabel();

signals:
    //sent when the mouse is moved
    void mouseMoved(QMouseEvent *event, bool mousePressed);
    //sent to track coordinates
    void coordinates(int x, int y);

protected:
    //tracks when the mouse is moved
    void mouseMoveEvent(QMouseEvent *event);
    //tracks when a mouse button is pressed
    void mousePressEvent(QMouseEvent *event);
    //tracks when a mouse button is released
    void mouseReleaseEvent(QMouseEvent *event);

private:
    //tracks when a mouse button is pressed
    bool mousePressed;
};

#endif // CLICKABLELABEL_H

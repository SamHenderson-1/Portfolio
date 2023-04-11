/*
Ben Rose, Grange Simpson, Tracy Garner, Krish Mahtani, Sam Henderson
CS3505
A7: Qt Sprite Editor
Reviewed by: Grange Simpson
This file sets up all the needed methods and variables for canvasmodel.cpp
*/
#ifndef CANVASMODEL_H
#define CANVASMODEL_H

#include <QWidget>
#include <QMouseEvent>
#include <QPainter>
#include <QImage>
#include <QPoint>
#include <QLabel>
#include "sprite.h"
#include <QDebug>
#include <QTimer>
#include <QColorDialog>

///@brief the CanvasModel controls the main logic behind the program
///         it connects the rest of the program to the user interface
/// @inherits QWidget
class CanvasModel : public QWidget
{
    //Qt Macro for creating a QObject
    Q_OBJECT
public:
    //the default constructor
    explicit CanvasModel(QWidget *parent = nullptr);
    //destructor
    ~CanvasModel();
    //a custom enumerator for tools
    enum possible_tool {PEN, ERASER, EYEDROPPER};
    //the sprite retriever
    Sprite* getSprite();
    //loads the sprite from a string
    void loadSpriteFromString(std::string sspString);
    //uses the enumerator to set a tool
    void setCurrentTool(possible_tool);
    //saves the sprite using a filename
    void save(std::string saveFile);
    //creates a new sprite with size length
    void createNewSprite(int length);
    //retrieves the current frame
    int getCurrentFrame();
    //sets the current frame from the vector
    void setCurrentFrame(int index);
    //refreshes the canvas
    void resetDisplay();

public slots:
    //recieves the new frameSize
    void setFrameSize(int frameSize);
    //creates a new frame
    void newFrame();
    //triggered when the mouse moves, checks if a button was pressed
    void mouseMoved(QMouseEvent *event, bool mousePressed);
    //animation is turned on or off
    void toggleAnimation(bool toggled);
    //sets the speed of the animation
    void setFPS(int);
    //changes the color of the pen
    void changePenColorButtonPressed();
    //switches to the brush tool
    void brushSelected();
    //switches to the eraser tool
    void eraserSelected();
    //switches to the eyedropper tool
    void eyeDropperSelected();
    void previousFrameViewSelected();
    void goForwardOneFrame();
    //goes to the previous frame
    void goBackwardOneFrame();
    //shows the previous frame for refrence for drawing
    void togglePreviousFrame();



private:
    //saves the currentFrame
    int currentFrame;
    //the flag for showing hte previous frame
    bool showPreviousFrame;
    //the tool which is currently being used
    possible_tool currentTool;
    //saves the backgroundColor of the canvas
    QColor backgroundColor;
    //saves the sprite as a pointer
    Sprite *sprite;
    //saves the canvas
    QPixmap canvas;
    //shows the previous frame on a small canvas
    QPixmap previousFrameCanvas;
    //saves the mouse position as a point
    QPoint mousePosition;
    //frames are saved in a vector
    vector<QPixmap> frameImages;
    //saves the animation play state
    bool animationToggled;
    //sets the timer for the amimating preview
    QTimer previewTimer;
    //keeps track of the frames per second
    int fps;
    //counter for keeps track of which frame to play, needs % size
    long animationFrameCounter;
    //custom palette for selecting colors for the pen
    QColorDialog *colorSelector;
    //the current color of the pen
    QColor penColor;
    //starts the animation preview
    void playPreview();
    //returns the tool that is selected
    void selectedTool();
    // when a frame is edited it updates the frame and vector for animation
    QPixmap redrawFrame(int frameIndex);

signals:
    //sends an updated canvas pixmap to the canvas
    QPixmap emitNewPixmap(QPixmap newPixmap);
    //sends the updated previous frame pixmap
    QPixmap emitNewPreviewPixmap(QPixmap newPreviewPixmap);
    //sends the next frame as a pixmap
    QPixmap nextPreviewFrame(QPixmap nextFrame);
    //sends the previous frame to the canvas
    QPixmap nextAnimationFrame(QPixmap nextFrame);
    //sends the current coordinates to the view
    void coordinates(const QString&);
    //sends the current canvas size to the view
    void changeSizeInView(int);
    //sends the current frame number to the view
    void showFrameNumber(int currentFrame);
};

#endif // CANVASMODEL_H

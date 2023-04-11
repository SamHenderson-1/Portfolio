#include <QFile>
#include <QPixmap>
#include <QPainter>
#include "canvasmodel.h"
#include <cmath>
#include <QDebug>
#include <iostream>
#include <QPainter>

CanvasModel::CanvasModel(QWidget *parent): QWidget{parent} {
    currentTool = PEN;
    backgroundColor = Qt::white;
    currentFrame = 0;
    showPreviousFrame = false;
    sprite = new Sprite(16);

    colorSelector = new QColorDialog(Qt::black,parent);
    penColor = Qt::black;
    setCurrentTool(PEN);

    animationToggled = false;
    connect(&previewTimer, &QTimer::timeout, this, &CanvasModel::playPreview);
}

void CanvasModel::resetDisplay() {
    if (frameImages.size() == 0) {
        newFrame();
    }
    else {
        canvas = frameImages[0];
        currentFrame = 0;
    }
    previewTimer.start(1000/fps);
    emit showFrameNumber(currentFrame);
}

CanvasModel::~CanvasModel(){
    delete sprite;
    delete colorSelector;
}

Sprite* CanvasModel::getSprite() {
    return sprite;
}

void CanvasModel::loadSpriteFromString(std::string sspString) {
     //load inside sprite
    sprite = new Sprite(sspString);
    frameImages.clear();
    for(int i = 0; i < sprite->getFrameCount(); i++) {
        frameImages.push_back(redrawFrame(i));
    }
    canvas = frameImages[0];
    setCurrentFrame(0);
    emit emitNewPixmap(canvas);
    emit changeSizeInView(sprite->getDimensions());
}

QPixmap CanvasModel::redrawFrame(int frameIndex) {
    double pixelSize = (double)300 / (double)sprite->getDimensions();
    QPixmap newFrame(300,300);
    newFrame.fill();
    QPainter p(&newFrame);
    for(int x = 0; x < sprite->getDimensions(); x++) {
        for(int y = 0; y < sprite->getDimensions(); y++) {
            int xPos = ceil(x * pixelSize);
            int yPos = ceil(y * pixelSize);
            p.fillRect(xPos,yPos,ceil(pixelSize),ceil(pixelSize), sprite->getPixel(x, y, frameIndex));
        }
    }
    p.end();
    return newFrame;
}

void CanvasModel::setCurrentTool(possible_tool tool) {
    currentTool = tool;
}

void CanvasModel::brushSelected(){
    setCurrentTool(PEN);
}

void CanvasModel::eraserSelected(){
    setCurrentTool(ERASER);
}

void CanvasModel::eyeDropperSelected(){
    setCurrentTool(EYEDROPPER);
}

void CanvasModel::previousFrameViewSelected(){
    if (showPreviousFrame && currentFrame > 0) {
        previousFrameCanvas = QPixmap(300,300);
        previousFrameCanvas.fill(Qt::transparent);
        previousFrameCanvas = frameImages[getCurrentFrame() - 1];

        QPainter painter(&previousFrameCanvas);
        painter.setOpacity(0.2);
        painter.end();
        emit emitNewPreviewPixmap(previousFrameCanvas.scaled(70,70));
        //canvas = onionSkinCanvas;
        //emit emitNewPixmap(canvas);
    }
    else {
        previousFrameCanvas.fill(Qt::transparent);
        emit emitNewPreviewPixmap(previousFrameCanvas.scaled(70, 70));
    }

    return;
}

void CanvasModel::save(std::string saveFile) {
    sprite->save(saveFile);
}

void CanvasModel::createNewSprite(int length) {
    //new sprite with input side lenght
    sprite = new Sprite(length);
    frameImages.clear();
    newFrame();
}

int CanvasModel::getCurrentFrame() {
    return currentFrame;
}

void CanvasModel::setCurrentFrame(int frame) {
    currentFrame = frame;
    emit showFrameNumber(currentFrame);
}

void CanvasModel::newFrame(){
    sprite->addFrame();
    canvas = QPixmap(300,300);
    canvas.fill();
    frameImages.push_back(canvas);
    currentFrame = frameImages.size() - 1;
    //onionSkinningSelected();
    previousFrameViewSelected();
    emit emitNewPixmap(canvas);
    emit showFrameNumber(currentFrame);
}

void CanvasModel::togglePreviousFrame() {
    showPreviousFrame = !showPreviousFrame;
    previousFrameViewSelected();
}

void CanvasModel::setFrameSize(int frameSize) {
    sprite->resize(frameSize);
    frameImages.clear();
    for (int i = 0; i < sprite->getFrameCount(); i++) {
        frameImages.push_back(redrawFrame(i));
    }
    canvas = frameImages[0];
    setCurrentFrame(0);
    emit emitNewPixmap(canvas);
}

void CanvasModel::playPreview() {
    previewTimer.setInterval(1000/fps);
    if (frameImages.size() != 0) {
        int index = animationFrameCounter % frameImages.size();
        emit nextPreviewFrame(frameImages[index].scaled(70,70));
        if (animationToggled) {
            currentFrame = index;
            emit emitNewPixmap(frameImages[index]);
        }

        animationFrameCounter++;
    }
}

void CanvasModel::setFPS(int inputFPS) {
    fps = inputFPS;
}

void CanvasModel::toggleAnimation(bool toggled) {
    animationToggled = toggled;
    if (!toggled) {
        canvas = frameImages[currentFrame];
        emit emitNewPixmap(canvas);
    }
}

void CanvasModel::changePenColorButtonPressed(){
    //colorSelector->open();
    QColor previousPenColor = penColor;
    penColor = colorSelector->getColor();
    qDebug() << penColor;
    if(!penColor.isValid()) {
        penColor = previousPenColor;
     }
}

void CanvasModel::goForwardOneFrame() {
    if (currentFrame < (int) frameImages.size() - 1) {
        setCurrentFrame(currentFrame + 1);
        canvas = frameImages[getCurrentFrame()];
        previousFrameViewSelected();
        emit emitNewPixmap(canvas);
    }
}

void CanvasModel::goBackwardOneFrame() {
    if (currentFrame > 0) {
        setCurrentFrame(currentFrame - 1);
        canvas = frameImages[getCurrentFrame()];
        previousFrameViewSelected();
        emit emitNewPixmap(canvas);
    }
}

void CanvasModel::mouseMoved(QMouseEvent *event, bool mousePressed){
    //onionSkinningSelected();

    int dimension = sprite->getDimensions();
    QColor color;
    if(currentTool == PEN){
        color = penColor;
    }
    else if(currentTool == ERASER){
        color = backgroundColor;
    }
    /*
    else if(currentTool == EYEDROPPER){
        color = QColor::fromRgba(qRgba(0,0,0,0));
    }
    */

    double pixelSize = (double)300 / (double)dimension;

    mousePosition = event->pos();

    int xIndex = floor(mousePosition.x() / pixelSize);
    int yIndex = floor(mousePosition.y() / pixelSize);
    int xPos = ceil(xIndex * pixelSize);
    int yPos = ceil(yIndex * pixelSize);

    string coords = std::to_string(xIndex) + ", " + std::to_string(yIndex);
    emit coordinates(QString::fromStdString(coords));
    //qDebug() << xPos << ", " << yPos << "\n";

    if(mousePressed && !animationToggled) {
        if (currentTool == EYEDROPPER) {
            penColor = sprite->getPixel(xIndex, yIndex, currentFrame);
            //penColor = color;
            return;
        }

        QPainter p(&canvas);
        // Fixes accidentally drawing on the borders when clicking and dragging
        if (xPos >= 0 && yPos >= 0)
            p.fillRect(xPos,yPos,ceil(pixelSize),ceil(pixelSize), color);
        p.end();

        //Save in our sprite
        sprite->setPixel(xIndex, yIndex, currentFrame, color);

        frameImages[currentFrame] = canvas;
        emit emitNewPixmap(canvas);
    }
}


/*
 * Ben Rose, Grange Simpson, Tracy Garner, Krish Mahtani, Sam Henderson
 * CS 3505
 * A7: Qt Sprite Editor
 */
#ifndef SPRITE_H
#define SPRITE_H

#include <QJsonObject>
#include <QColor>
#include <string>
#include "frame.h"
using std::vector;
using std::string;

class Sprite
{

public:
    //creates a new Sprite with dimensions lenxlen. Pixels are white by default.
    Sprite(int len);
    // creates a Sprite based on the input JSON file. Reads pixel colors, frames, and
    // height and width.
    Sprite(string fileName);
    // returns width and height of the square sprite
    int getDimensions();
    // returns number of frames the sprite has stored
    int getFrameCount();
    // finds the color of a certain pixel
    QColor getPixel(int x, int y, int frame);
    // changes the color of the indicated pixel
    void setPixel(int x, int y, int frame, QColor color);
    // adds a new blank white frame to the Sprite, using the same dimensions
    void addFrame();
    // removes a frame at the indicated index
    void removeFrame(int frame);
    // writes the Sprite to a file with a .ssp extension, using JSON to save the data.
    // Stores RGBA values for the pixels, height and width, and number of frames.
    void save(string fileName);
    // changes the dimensions of each frame of the sprite. Resizing happens upward and
    // leftward, so the top left corner remains in place.
    void resize(int sideLength);

private:
    // sideLength holds the dimensions of the Sprite, sideLength x sideLength
    int sideLength;
    // holds the list of Frames in their animation order
    vector<Frame> frames;

    // helper method for Sprite::save, writes the information using JSON
    void write(QJsonObject &json);
    // helper method for building a Sprite out of an input JSON file. Uses the input
    // JSON listing the pixel data, height and width, and number of frames to fill in
    // the member variables of this object.
    void read(const QJsonObject &json);
    // helper for Sprite::read method that takes in a QJsonArray that lists out the
    // pixels and their RGBA values and uses it to set the pixels of this object.
    void setPixelsFromJsonArray(QJsonArray, int);
};

#endif // SPRITE_H
